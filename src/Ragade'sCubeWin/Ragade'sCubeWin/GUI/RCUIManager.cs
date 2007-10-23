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

            FindFirstFocus();
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

        /// <summary>
        /// Uses algorithm to find the next control in the direction
        /// relative to the focused control.
        /// </summary>
        /// <param name="direction"></param>
        private void MoveFocus(
            GUIMoveEvent.GUIMoveDirection direction
            )
        {
            if (_focusedObject == null)
            {
                if (!FindFirstFocus())
                {
                    return;
                }
            }

            
            Debug.Assert(_focusedObject != null);
               


            Vector3 moveDirection = Vector3.Zero;
            switch (direction)
            {
                case GUIMoveEvent.GUIMoveDirection.Up:
                    moveDirection = _focusedObject.WorldTrans.Up;
                    break;
                case GUIMoveEvent.GUIMoveDirection.Down:
                    moveDirection = _focusedObject.WorldTrans.Down;
                    break;
                case GUIMoveEvent.GUIMoveDirection.Left:
                    moveDirection = _focusedObject.WorldTrans.Left;
                    break;
                case GUIMoveEvent.GUIMoveDirection.Right:
                    moveDirection = _focusedObject.WorldTrans.Right;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            IGUIElement nextCandidate = null;
            float nextCandidateCloseness = float.MaxValue;
            foreach (IGUIElement element in _guiObjects)
            {
                if (element.AcceptsFocus)
                {
                    if (element != _focusedObject)
                    {
                        Vector3 orthoDirection = Vector3.Cross(
                            moveDirection, 
                            _focusedObject.WorldTrans.Forward
                            );


                        Vector3 dirToNext = element.WorldTrans.Translation -
                            _focusedObject.WorldTrans.Translation;

                        float compInMoveDir = Vector3.Dot(
                            moveDirection,
                            dirToNext
                            );

                        if (compInMoveDir > 0)
                        {

                            float compInOrthoDir = Vector3.Dot(
                                orthoDirection,
                                dirToNext
                                );

                            float candidateCloseness =
                                1 * compInMoveDir * compInMoveDir +
                                100 * compInOrthoDir * compInOrthoDir;

                            if (candidateCloseness < nextCandidateCloseness)
                            {
                                nextCandidate = element;
                                nextCandidateCloseness = candidateCloseness;
                            }
                        }
                    }
                }    
            }

            if (nextCandidate != null)
            {
                FocusElement(nextCandidate);
            }
            
        }

        private bool FindFirstFocus()
        {
            bool found = false;
            foreach (IGUIElement element in _guiObjects)
            {
                if (element.AcceptsFocus)
                {
                    FocusElement(element);
                    found = true;
                    break;
                }
            }

            return found;
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
