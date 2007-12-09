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
        private IGUIElement _lastClicked;
        
        private bool _focusLock;

        private bool _inputEnabled;

        public bool AcceptInput
        {
            get { return _inputEnabled; }
            set { _inputEnabled = value; }
        }

        public RCGUIManager(RCScene scene)
        {
            _scene = scene;
            _guiObjects = new List<IGUIElement>();
            _picker = new RCScenePicker();

            _focusLock = false;
            _inputEnabled = true;

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

        #region Move Functions
        public void MoveDown()
        {
         
            MoveEvent(GUIMoveEvent.GUIMoveDirection.Down);
        }
        public void MoveUp()
        {
         
            MoveEvent(GUIMoveEvent.GUIMoveDirection.Up);
        }
        public void MoveLeft()
        {
         
            MoveEvent(GUIMoveEvent.GUIMoveDirection.Left);
        }
        public void MoveRight()
        {
         
            MoveEvent(GUIMoveEvent.GUIMoveDirection.Right);
        }

        public void MoveEvent(GUIMoveEvent.GUIMoveDirection direction)
        {
            SoundManagement.SoundManager.PlaySound("blip");
            GuiInputEvent(new GUIMoveEvent(
                    direction,
                    this
                    ));
        }

        #endregion 

        #region Selection Functions
        public void AcceptFocused()
        {
            SelectFocused(GUISelectEvent.GUISelectType.Accept);
        }
        public void DeclineFocused()
        {
            SelectFocused(GUISelectEvent.GUISelectType.Decline);
        }

        private void SelectElement(
            IGUIElement element,
            GUISelectEvent.GUISelectType type
            )
        {
            if (AcceptInput)
            {
                if (element != null)
                {
                    element.OnEvent(new GUISelectEvent(
                         type,
                         this
                         ));
                }
            }
        }

        private void SelectFocused(GUISelectEvent.GUISelectType type)
        {
            SelectElement(_focusedObject, type);
        }

        #endregion 


        // Forward input events on to GUIElements
        public void GuiInputEvent(GUIEvent inputEvent)
        {
            if (AcceptInput)
            {
               
                if (inputEvent is GUIMouseEvent)
                {
                    OnMouseEvent(inputEvent);
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
            }
        }

        private void OnMouseEvent(GUIEvent inputEvent)
        {
            GUIMouseEvent mouseEvent = (GUIMouseEvent)inputEvent;

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
                        _lastClicked = e;
                    }

                    // Send event on to control
                    e.OnEvent(inputEvent);
                }
            }
            else if (mouseEvent.MouseEventType == GUIMouseEvent.GUIMouseEventType.MouseUp)
            {
                if (_lastClicked != null)
                {
                    List<ISpatial> listElements = RetriveElements(mouseEvent.Point);

                    // Find elements under the cursor.
                    foreach (ISpatial element in listElements)
                    {
                        Debug.Assert(element is IGUIElement);

                        IGUIElement e = (IGUIElement)element;

                        // If the mouse button comes up on the control that
                        // it came down on, Send an accept event to it.

                        // Also uncache the last clicked item.
                        if (e == _lastClicked)
                        {
                            // Send accept the item.
                            SelectElement(e, GUISelectEvent.GUISelectType.Accept);
                            _lastClicked = null;
                        }
                        
                        // Send mouse up event to all affected controls.
                        e.OnEvent(mouseEvent);
                    }

                    // If previously clicked button did not have a mouse
                    // buttom come up while the cursor is on it. Assume the
                    // user wanted to decline the button.
                    if (_lastClicked != null)
                    {
                        SelectElement(_lastClicked, GUISelectEvent.GUISelectType.Decline);
                        _lastClicked = null;
                    }       
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

                            // Exclude candidates that are essentailly entrily in the ortho direction.
                            if (Math.Abs(compInOrthoDir) / dirToNext.Length() > .995)
                            {
                                continue;
                            }

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
            if (element == null)
            {
                return;
            }
            
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

        public void CaptureFocus(IGUIElement element)
        {
            if (element == null)
            {
                return;
            }

            FocusElement(element);
            _focusLock = true;
        }

        public void ReleaseFocus()
        {
            _focusLock = false;
        }


        public bool IsFocused(IGUIElement element)
        {
            if (element == null)
            {
                return false;
            }

            return _focusedObject != null && _focusedObject == element;
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
