using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Picking;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GUI.Panes;
using System.Diagnostics;
namespace RagadesCubeWin.GUI
{
    public class RCGUIManager
    {
        RCScene _scene;
        private List<IGUIElement> _guiObjects;
        private RCScenePicker _picker;

        private IGUIElement _focusedObject;

        public RCGUIManager(RCScene scene)
        {
            _scene = scene;
            _guiObjects = new List<IGUIElement>();
            _picker = new RCScenePicker();

            RebuildGuiObjectList();
        }

        public void RebuildGuiObjectList()
        {
            _guiObjects.Clear();

            BuildGuiObjectListRecursive(_scene.SceneRoot);

            foreach (IGUIElement element in _guiObjects)
            {
                if (element.AcceptsFocus)
                {
                    FocusElement(element);
                    break;
                }
            }
        }

        private void BuildGuiObjectListRecursive(ISpatial root)
        {
            // Find all gui elements under this root.
            // First check to see if it is a GUIElement itself
            if (root is IGUIElement)
            {
                _guiObjects.Add((IGUIElement)root);
            }

            // Check root's children are GUIElements, if any.
            if (root is INode)
            {
                INode rootNode = (INode)root;

                List<ISpatial> listChildren = rootNode.GetChildren();

                foreach (ISpatial child in listChildren)
                {
                    BuildGuiObjectListRecursive(child);
                }
            }
        }

        // Forward input events on to GUIElements
        public void GuiInputEvent(GUIEvent inputEvent)
        {
            if (inputEvent is GUIMouseEvent)
            {
                GUIMouseEvent mouseEvent =(GUIMouseEvent)inputEvent;

                if (mouseEvent.MouseEventType == GUIMouseEvent.GUIMouseEventType.MouseDown)
                {
                    List<ISpatial> listElements = RetriveElements(mouseEvent.Point);

                    foreach (ISpatial element in listElements)
                    {
                        Debug.Assert(element is IGUIElement);

                        IGUIElement e = (IGUIElement)element;

                        if (e.AcceptsFocus)
                        {
                            FocusElement(e);
                        }

                        // Send event on to control
                        e.OnEvent(inputEvent);
                    }
                }
                else if (mouseEvent.MouseEventType == GUIMouseEvent.GUIMouseEventType.MouseUp)
                {
                    if (_focusedObject != null)
                    {
                        _focusedObject.OnEvent(mouseEvent);
                    }
                }
            }
            else if (inputEvent is GUIMoveEvent)
            {
                bool handled = false;
                if (_focusedObject != null)
                {
                    handled = _focusedObject.OnEvent(inputEvent);
                }

                if (!handled)
                {
                    GUIMoveEvent moveEvent = (GUIMoveEvent)inputEvent;
                    MoveFocus(moveEvent.Direction);
                }
            }
            else if (inputEvent is GUIKeyEvent)
            {
                if (_focusedObject != null)
                {
                    _focusedObject.OnEvent(inputEvent);
                }
            }
            else if (inputEvent is GUISelectEvent)
            {
                if (_focusedObject != null)
                {
                    _focusedObject.OnEvent(inputEvent);
                }
            }
        }

        private void MoveFocus(
            GUIMoveEvent.GUIMoveDirection direction
            )
        {
            Vector3 moveDirection = Vector3.Zero;
            switch (direction)
            {
                case GUIMoveEvent.GUIMoveDirection.Up:
                    moveDirection = Vector3.Up;
                    break;
                case GUIMoveEvent.GUIMoveDirection.Down:
                    moveDirection = Vector3.Down;
                    break;
                case GUIMoveEvent.GUIMoveDirection.Left:
                    moveDirection = Vector3.Left;
                    break;
                case GUIMoveEvent.GUIMoveDirection.Right:
                    moveDirection = Vector3.Right;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            IGUIElement nextCandidate = null;
            float nextCandidateDotProd = 0;
            foreach (IGUIElement element in _guiObjects)
            {
                if (element.AcceptsFocus)
                {
                    if (_focusedObject != null)
                    {
                        Vector3 dirToNext = element.WorldTrans.Translation -
                            _focusedObject.WorldTrans.Translation;
                        dirToNext.Normalize();

                        // Find the control who's location is most in the
                        // specified direction from the focued control.
                        float dotProd = Vector3.Dot(dirToNext, moveDirection);
                        if (dotProd > nextCandidateDotProd)
                        {
                            nextCandidate = element;
                            nextCandidateDotProd = dotProd;
                        }
                    }
                    else
                    {
                        FocusElement(element);
                        break;
                    }
                }
            }
            
        }

        private void FocusElement(IGUIElement element)
        {
            if (_focusedObject != null)
            {
                
                _focusedObject.OnEvent(
                    new GUIFocusEvent(
                        GUIFocusEvent.GUIFocusType.Unfocused,
                        this
                        )
                    );
            }

            if (element.AcceptsFocus)
            {
                _focusedObject = element;

                _focusedObject.OnEvent(
                    new GUIFocusEvent(
                        GUIFocusEvent.GUIFocusType.Focused,
                        this
                        )
                    );
            }

        }

        private List<ISpatial> RetriveElements(Point scrCoords)
        {
            RCPickRecord record = _picker.Pick(scrCoords, _scene);

            record.AddFilter(
                typeof(IGUIElement),
                RCPickRecord.FilterMode.KeepCompatible
                );

            // Get the item closest to the screen.
            List<ISpatial> selected = record.GetPicked();

            return selected;
        }
    }
}
