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

        /// <summary>
        /// The internal Z-order of the base image, used to determine which images
        /// are drawn over which other images.  The base image should be on the
        /// bottom, as it should have the highest opacity.
        /// </summary>
        protected float BASE_IMAGE_Z_ORDER = 0f;
        /// <summary>
        /// The internal Z-order of the focus image, used to determine which images
        /// are drawn over which other images.  The focus image should probably be 
        /// in the middle, as it should be less visible than the activating image.
        /// </summary>
        protected float FOCUSED_IMAGE_Z_ORDER = 0.025f;
        /// <summary>
        /// The internal Z-order of the 'pushing' image, used to determine which images
        /// are drawn over which other images.  The 'pushing' image should probably be 
        /// on the top, as it should be the most visible effect.
        /// </summary>
        protected float ACTIVATING_IMAGE_Z_ORDER = 0.05f;

        #endregion     ------------------------------Local Constants

        /// <summary>
        /// The private RCText object that comprises the button's text.
        /// </summary>
        private RCText textObject = null;

        [needsXML]
        private bool _thinksItHasFocus = false;
        [needsXML]
        private bool _thinksItIsBeingPushed = false;

        /// <summary>
        /// The image overlay that indicates that the RCButton is focused.
        /// This should be transparent.
        /// </summary>
        private RCQuad focusedImageObject = null;

        /// <summary>
        /// The image overlay that indicates that the RCButton is being pushed.
        /// This should be transparent.
        /// </summary>
        private RCQuad activatingImageObject = null;

        /// <summary>
        /// The protected RCQuad object that represents the object in question with absolutely no modification.
        /// </summary>
        private RCQuad baseImageObject = null;


        [needsXML]
        private string _baseImageObjectFileNameMinusExtension = "roughButtonImage";
        [needsXML]
        private string _focusedImageObjectFileNameMinusExtension = "roughSelectedButtonImage";
        [needsXML]
        private string _activatingImageObjectFileNameMinusExtension = "roughPressedButtonImageDepressed";

        #endregion ------------------------------Private data members

        #region    ------------------------------Events and their delegates
        /// <summary>
        /// The type of function that an RCButton's events call; void and parameterless.
        /// </summary>
        internal delegate void RCButtonEvent();
        /// <summary>
        /// The event to throw when the RCButton begins to be pressed.
        /// </summary>
        internal event RCButtonEvent WhilePressing = null;
        /// <summary>
        /// The event to throw when the RCButton begins to be released, if the mouse
        /// is hovering over it.
        /// </summary>
        internal event RCButtonEvent WhileReleasing = null;
        /// <summary>
        /// The event to throw when the RCButton has been pressed and released.
        /// </summary>
        internal event RCButtonEvent AfterPressedAndReleased = null;
        /// <summary>
        /// The event to throw when the RCButton takes focus.
        /// </summary>
        internal event RCButtonEvent Focus = null;
        /// <summary>
        /// The event to throw when the RCButton loses focus.
        /// </summary>
        internal event RCButtonEvent UnFocus = null;

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

        [needsXML]
        public string baseImageFileName
        {
            get { return _baseImageObjectFileNameMinusExtension; }
            set { _baseImageObjectFileNameMinusExtension = value; }
        }

        [needsXML]
        public string focusedImageFileName
        {
            get { return _focusedImageObjectFileNameMinusExtension; }
            set { _focusedImageObjectFileNameMinusExtension = value; }
        }

        [needsXML]
        public string activatingImageFileName
        {
            get { return _activatingImageObjectFileNameMinusExtension; }
            set { _activatingImageObjectFileNameMinusExtension = value; }
        }

        #endregion ------------------------------Public properties to access objects contained within

        #region    ------------------------------Public properties to return information regarding the state of the button
        
        [notFullyImplemented("Currently keeps track of focus internally, instead of looking at actual focusing properties.")]
        /// <summary>
        /// Returns true if the RCButton is currently being focused, and false if it is not.
        /// </summary>
        public bool isFocused
        {
            get
            { return _thinksItHasFocus; }
        }

        /// <summary>
        /// Returns true if the RCButton is currently depressed, and false if it is not.
        /// </summary>
        public bool isBeingPushed
        {
            get { return _thinksItIsBeingPushed; }
        }
        #endregion    ------------------------------Public properties to return information regarding the state of the button


        
        


        /// <summary>
        /// Creates a new RCButton object.  This needs to be attached to something for it to be visible.
        /// Note that this is a 'flat 3-d' object; it has a very small 'length' property by definition.
        /// </summary>
        /// <param name="width">The width of the object in world-coordinates.</param>
        /// <param name="height">The height of the object in world-coordinates.</param>
        /// <param name="screenWidth">The width of the object in screen-coordinates.</param>
        /// <param name="screenHeight">The height of the object in screen-coordinates.</param>
        /// <param name="Font">The BitmapFont that the button's text uses.</param>
        [placeHolder]
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
            textObject.Text = "";
            AddChild(textObject, 15, 15, 0.075f);
            AcceptsFocus = true;

            baseImageObject = new RCQuad(width, height, screenWidth, screenHeight);
            
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

        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadGraphicsContent(graphics, content);
            baseImageObject.Image = content.Load<Texture2D>("Content\\Textures\\" + _baseImageObjectFileNameMinusExtension);
            focusedImageObject.Image = content.Load<Texture2D>("Content\\Textures\\" + _focusedImageObjectFileNameMinusExtension);
            activatingImageObject.Image = content.Load<Texture2D>("Content\\Textures\\" + _activatingImageObjectFileNameMinusExtension);
            //if (currentImageObject.Image == null)
            //{ currentImageObject.Image = baseImageObject.Image; }
        }


        public override void UnloadGraphicsContent()
        {
            baseImageObject = null;
            base.UnloadGraphicsContent();
        }
        
        #endregion ------------------------------Overridden functions


        /// <summary>
        /// The internal logic associated with focusing.  Includes animation and setting of local variables.
        /// You should never have to mess with this, but it's public in case you want to.
        /// </summary>
        public void FocusInternalLogic()
        {
            _thinksItHasFocus = true;
            if (!base._listChildren.Contains(focusedImageObject))
            {
                AddChild(focusedImageObject, 0, 0, FOCUSED_IMAGE_Z_ORDER);
            }
        }

        /// The internal logic associated with unfocusing.  Includes animation and setting of local variables.
        /// You should never have to mess with this, but it's public in case you want to.
        public void UnFocusInternalLogic()
        {
            _thinksItHasFocus = false;
            RemoveChild(focusedImageObject);
        }

        /// The internal logic associated with depressing.  Includes animation and setting of local variables.
        /// You should never have to mess with this, but it's public in case you want to.
        public void PressingInternalLogic()
        {
            _thinksItIsBeingPushed = true;
            if (!base._listChildren.Contains(activatingImageObject))
            {
                AddChild(activatingImageObject, 0, 0, ACTIVATING_IMAGE_Z_ORDER);
            }
        }

        /// The internal logic associated with un-depressing.  Includes animation and setting of local variables.
        /// You should never have to mess with this, but it's public in case you want to.
        public void UnPressingInternalLogic()
        {
            _thinksItIsBeingPushed = false;
           RemoveChild(activatingImageObject);
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
                        ThrowLocalEvent(WhilePressing);
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
                
                //this.textObject.Text = keyEvent.Key.ToString();
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
                        ThrowLocalEvent(AfterPressedAndReleased);
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
        public bool ThrowLocalEvent(RCButtonEvent eventToThrow)
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
    