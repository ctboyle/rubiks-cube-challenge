using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GUI
{
    
    public abstract class GUIEvent
    {
        public enum GUIEventType
        {
            Focus,
            Select,
            Move,
            MouseEvent,
            Key
        }

        private GUIEventType _type;
        private RCGUIManager _manager;

        public abstract GUIEventType Type
        {
            get;
        }

        public RCGUIManager GUIManager
        {
            get { return _manager;}
        }

        public GUIEvent(RCGUIManager manager)
        {
            _manager = manager;
        }

    }

    public class GUIKeyEvent : GUIEvent
    {
        private Keys _key;
        
        public override GUIEventType Type
        {
            get { return GUIEventType.Key; }
        }

        public Keys Key
        {
            get { return _key; }

        }

        public GUIKeyEvent(
            Keys key,
            RCGUIManager manager
            )
            :base(manager)
        {
            _key = key;
        }
    }

    public class GUIFocusEvent : GUIEvent
    {
        public enum GUIFocusType
        {
            Focused,
            Unfocused
        }
        private GUIFocusType _focusEvent;

        public override GUIEventType Type
        {
            get { return GUIEventType.Focus; }
        }

        public GUIFocusType FocusEvent
        {
            get {return _focusEvent;}
        }

        public GUIFocusEvent(
            GUIFocusType focusEvent,
            RCGUIManager manager
            )
            :base(manager)
        {
            _focusEvent = focusEvent;
        }
    }

    public class GUISelectEvent : GUIEvent
    {
        public enum GUISelectType
        {
            Ok,
            Cancel
        }
        private GUISelectType _selectEvent;

        public override GUIEventType Type
        {
            get { return GUIEventType.Select; }
        }

        public GUISelectType SelectEvent
        {
            get {return _selectEvent;}
        }

        public GUISelectEvent(
            GUISelectType selectEvent,
            RCGUIManager manager
            )
            :base(manager)
        {
            _selectEvent = selectEvent;
        }
    }

    public class GUIMoveEvent : GUIEvent
    {
        public enum GUIMoveDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        private GUIMoveDirection _direction;

        public override GUIEvent.GUIEventType Type
        {
            get { return GUIEventType.Move; }
        }

        public GUIMoveDirection Direction
        {
            get { return _direction; }
        }

        public GUIMoveEvent(
            GUIMoveDirection direction,
            RCGUIManager manager
            )
            : base(manager)
        {
            _direction = direction;
        }

        
    }

    public class GUIMouseEvent : GUIEvent
    {
        public enum GUIMouseEventType
        {
            MouseDown,
            MouseUp,
            MouseDrag
        }

        private int _xPos;
        private int _yPos;
        private GUIMouseEventType _mouseEvent;

        public override GUIEventType Type
        {
            get { return GUIEventType.MouseEvent; }
        }

        public GUIMouseEventType MouseEventType
        {
            get { return _mouseEvent; }
        }

        public Point Point
        {
            get { return new Point(_xPos, _yPos); }
        }

        public GUIMouseEvent(
            GUIMouseEventType mouseEventType,
            int xLocalPos,
            int yLocalPos,
            RCGUIManager manager
            )
            : base(manager)
        {
            _mouseEvent = mouseEventType;
            _xPos = xLocalPos;
            _yPos = yLocalPos;
        }

    }

    


}
