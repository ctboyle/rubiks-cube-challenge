using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.GUI.Fonts;
using Microsoft.Xna.Framework.Graphics;

namespace RagadesCubeWin.GUI
{
    [placeHolder]
    [needsXML]
    class RCButton : RCControl
    {
        #region    ------------------------------Private data members

        #region        ------------------------------Local Constants

        protected float BASE_IMAGE_Z_ORDER = 0f;
        protected float FOCUSED_IMAGE_Z_ORDER = 0.1f;
        protected float ACTIVATING_IMAGE_Z_ORDER = 0.3f;

        #endregion     ------------------------------Local Constants

        /// <summary>
        /// The private RCText object that comprises the button's text.
        /// </summary>
        private RCText textObject = null;

        [needsXML]
        private bool _thinksItHasFocus = false;
        [needsXML]
        private bool _thinksItIsBeingPushed = false;

        [needsXML]
        private RCQuad focusedImageObject = null;

        [needsXML]
        private RCQuad activatingImageObject = null;

        #endregion ------------------------------Private data members

        #region    ------------------------------Events and their delegates
        [needsXML]
        internal delegate void RCButtonEvent();
        [needsXML]
        internal event RCButtonEvent WhilePressing = null;
        [needsXML]
        internal event RCButtonEvent WhileReleasing = null;
        [needsXML]
        internal event RCButtonEvent AfterPressedAndReleased = null;
        [needsXML]
        internal event RCButtonEvent Focus = null;
        [needsXML]
        internal event RCButtonEvent UnFocus = null;
        [needsXML]

        #endregion ------------------------------Events and their delegates

        #region    ------------------------------Public properties to access objects contained within


        /// <summary>
        /// Sets or returns the RCText object that comprises the button's text
        /// </summary>
        public RCText buttonText
        {
            get { return textObject; }
            set { textObject = value; }
        }

        #endregion ------------------------------Public properties to access objects contained within


        #region    ------------------------------Public properties to return information regarding the state of the button
        [needsXML]
        public bool isFocused
        {
            get
            { return _thinksItHasFocus; }
        }

        [needsXML]
        public bool isBeingPushed
        {
            get { return _thinksItIsBeingPushed; }
        }
        #endregion    ------------------------------Public properties to return information regarding the state of the button


        #region    ------------------------------Enumerations for RCButton
        [needsXML]
        [scaffolding]
        enum RCButtonState
        {
            [needsXML]
            normal = 0,
            [needsXML]
            invisible = 1,
            [needsXML]
            disabled = 2,
            [needsXML]
            focused = 4,
            [needsXML]
            pressed = 8,
            [needsXML]
            pressing = 16
        }
        #endregion ------------------------------Enumerations for RCButton


        
        



        [placeHolder]
        [needsXML]
        internal RCButton(
            float width, 
            float height,
            int screenWidth, 
            int screenHeight,
            BitmapFont Font
        ) : base(
            width,
            height,
            screenWidth,
            screenHeight
        )
        {
            textObject = new RCText(Font, width, height, screenWidth, screenHeight);
            textObject.Text = "Nameless Button";
            AddChild(textObject, 15, 15, 0.1f);
            AcceptsFocus = true;

            
            //            AddChild(selectedImageObject, 0, 0, 0f);
            
            focusedImageObject = new RCQuad(width, height, screenWidth, screenHeight);
//            AddChild(selectedImageObject, 0, 0, 0f);

            activatingImageObject = new RCQuad(width, height, screenWidth, screenHeight);
//            AddChild(activatingImageObject, 0, 0, 0f);

            //currentImageObject.Image = baseImageObject.Image;
            //AddChild(currentImageObject, 0, 0, 0f);
            AddChild(baseImageObject, 0, 0, 0f);

        }

        #region    ------------------------------Overridden functions

        [needsXML]
        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadGraphicsContent(graphics, content);
            baseImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughButtonImage");
            focusedImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughSelectedButtonImage");
            activatingImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughPressedButtonImageDepressed");
            //if (currentImageObject.Image == null)
            //{ currentImageObject.Image = baseImageObject.Image; }
        }
        
        
       
         

        #endregion ------------------------------Overridden functions

        [needsXML]
        public void FocusInternalLogic()
        {
            _thinksItHasFocus = true;
            if (!base._listChildren.Contains(focusedImageObject))
            {
                AddChild(focusedImageObject, 0, 0, FOCUSED_IMAGE_Z_ORDER);
            }
        }

        [needsXML]
        public void UnFocusInternalLogic()
        {
            _thinksItHasFocus = false;
            RemoveChild(focusedImageObject);
        }

        [needsXML]
        public void PressingInternalLogic()
        {
            _thinksItIsBeingPushed = true;
            if (!base._listChildren.Contains(activatingImageObject))
            {
                AddChild(activatingImageObject, 0, 0, ACTIVATING_IMAGE_Z_ORDER);
            }
        }

        [needsXML]
        public void UnPressingInternalLogic()
        {
            _thinksItIsBeingPushed = false;
           RemoveChild(activatingImageObject);
        }

        [needsXML]
        protected override void instantiateBaseAndCurrentImageObjects(float width, float height, int screenWidth, int screenHeight)
        {
            baseImageObject = new RCQuad(width, height, screenWidth, screenHeight);
            //currentImageObject = new RCQuad(width, height, screenWidth, screenHeight);
        }

        public override bool OnEvent(GUIEvent guiEvent)
        {
            bool handled = false;
            if (guiEvent is GUIMouseEvent)
            {
                GUIMouseEvent mouseEvent = (GUIMouseEvent)guiEvent;

                switch (mouseEvent.MouseEventType)
                {
                    case GUIMouseEvent.GUIMouseEventType.MouseDown:
                        PressingInternalLogic();
                        throwLocalEvent(WhilePressing);
                        break;

                    case GUIMouseEvent.GUIMouseEventType.MouseUp:
                        //Nothing happening if mouse button is released
                        //  (but note that if the mouse button had been
                        //   pressed over this control, a GUIFocusType
                        //   event will be thrown.
                        break;
                }

                handled = true;
            }
            else if (guiEvent is GUIFocusEvent)
            {
                GUIFocusEvent focusEvent = (GUIFocusEvent)guiEvent;

                switch (focusEvent.FocusEvent)
                {
                    case GUIFocusEvent.GUIFocusType.Focused:
                        FocusInternalLogic();
                        break;
                    case GUIFocusEvent.GUIFocusType.Unfocused:
                        UnFocusInternalLogic();
                        break;
                }

                handled = true;
            }
            else if (guiEvent is GUIKeyEvent)
            {
            GUIKeyEvent keyEvent = (GUIKeyEvent)guiEvent;
                
                this.textObject.Text = keyEvent.Key.ToString();
            }
            else if (guiEvent is GUIMoveEvent)
            {
                GUIMoveEvent moveEvent = (GUIMoveEvent)guiEvent;
                switch (moveEvent.Direction)
                {
                    case GUIMoveEvent.GUIMoveDirection.Down:
                        break;
                    case GUIMoveEvent.GUIMoveDirection.Left:
                        break;
                    case GUIMoveEvent.GUIMoveDirection.Right:
                        break;
                    case GUIMoveEvent.GUIMoveDirection.Up:
                        break;
                }
            }
            else if (guiEvent is GUISelectEvent)
            {
                UnPressingInternalLogic();

                GUISelectEvent selectEvent = (GUISelectEvent)guiEvent;
                
                switch (selectEvent.SelectEvent)
                {
                    case GUISelectEvent.GUISelectType.Accept:
                        throwLocalEvent(AfterPressedAndReleased);
                        break;
                    case GUISelectEvent.GUISelectType.Decline:
                        break;
                }

            }
            return handled;
        }

        /// <summary>
        /// Attempts to handle the specified local event, and returns if there is a handler set.
        /// </summary>
        /// <param name="eventToThrow">The event to attempt to throw.</param>
        /// <returns>Whether or not <paramref name="eventToThrow"/> was handled (true if it did, false if it did not).</returns>
        public bool throwLocalEvent(RCButtonEvent eventToThrow)
        {
            if (eventToThrow == null)
            { return false; }
            else
            {
                eventToThrow();
                return true;
            }
        }


    }
}
    