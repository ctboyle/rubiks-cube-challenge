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

        /// <summary>
        /// The private RCText object that comprises the button's text.
        /// </summary>
        private RCText textObject = null;


        [needsXML]
        private RCQuad selectedImageObject = null;

        [needsXML]
        private RCQuad activatingImageObject = null;

        #endregion ------------------------------Private data members

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


        #region    ------------------------------Enumerations for RCButton
        [needsXML]
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
            

            
            //            AddChild(selectedImageObject, 0, 0, 0f);
            
            selectedImageObject = new RCQuad(width, height, screenWidth, screenHeight);
//            AddChild(selectedImageObject, 0, 0, 0f);

            activatingImageObject = new RCQuad(width, height, screenWidth, screenHeight);
//            AddChild(activatingImageObject, 0, 0, 0f);

            currentImageObject.Image = baseImageObject.Image;
            AddChild(currentImageObject, 0, 0, 0f);

        }

        #region    ------------------------------Overridden functions



        [needsXML]
        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadGraphicsContent(graphics, content);
            baseImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughButtonImage");
            selectedImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughSelectedButtonImage");
            activatingImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughPressedButtonImageDepressed");
            if (currentImageObject.Image == null)
            { currentImageObject.Image = baseImageObject.Image; }
        }
        
        
       
         

        #endregion ------------------------------Overridden functions

        [needsXML]
        public void Focus()
        {
            currentImageObject.Image = selectedImageObject.Image;
        }

        [needsXML]
        public void UnFocus()
        {
            currentImageObject.Image = baseImageObject.Image;
        }

        [needsXML]
        public void Pressing()
        {
            currentImageObject.Image = activatingImageObject.Image;
        }

        [needsXML]
        public void UnPressing()
        {
            currentImageObject.Image = baseImageObject.Image;
        }

        [needsXML]
        protected override void instantiateBaseAndCurrentImageObjects(float width, float height, int screenWidth, int screenHeight)
        {
            baseImageObject = new RCQuad(width, height, screenWidth, screenHeight);
            currentImageObject = new RCQuad(width, height, screenWidth, screenHeight);
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
                        Pressing();
                        break;

                    case GUIMouseEvent.GUIMouseEventType.MouseUp:
                        Focus();
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
                        Focus();
                        break;
                    case GUIFocusEvent.GUIFocusType.Unfocused:
                        UnFocus();
                        break;
                }

                handled = true;
            }
            return handled;
        }


    }
}
