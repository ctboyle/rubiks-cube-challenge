using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GUI.Primitives;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.GUI.Fonts;

namespace RagadesCubeWin.GUI.Controls.Control_Subclasses
{
    class TestControl : RCPane
    {
        public delegate void ClickHandler();
        public event ClickHandler Accepted;

        /// <summary>
        /// The private RCText object that comprises the button's text.
        /// </summary>
        private RCText textObject;

        /// <summary>
        /// The private RCQuad object that is being displayed at the current time.
        /// </summary>
        private RCQuad imageQuad = null;


        Texture2D _pressedImage;
        Texture2D _baseImage;
        Texture2D _focusedImage;

        /// <summary>
        /// Sets or returns the RCText object that comprises the button's text
        /// </summary>
        public string Text
        {
            get 
            {
                if (textObject != null)
                {
                    return textObject.Text;
                }
                return null;
            }
            set
            {
                if (textObject != null)
                {
                    textObject.Text = value;
                }
            }
        }

        [placeHolder]
        [needsXML]
        internal TestControl(
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

            AcceptsFocus = true;

            textObject = new RCText(Font, width, height, screenWidth, screenHeight);
            textObject.Text = "Test Control";
            AddChild(textObject, 15, 15, 0.1f);
            

            imageQuad = new RCQuad(width, height, screenWidth, screenHeight);
            AddChild(imageQuad, 0, 0, 0f);

        }

        public override void Draw(GraphicsDevice graphicsDevice)
        {
            base.Draw(graphicsDevice);
        }
  
        [needsXML]
        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadGraphicsContent(graphics, content);
            _baseImage = content.Load<Texture2D>("Content\\Textures\\roughButtonImage");
            _focusedImage = content.Load<Texture2D>("Content\\Textures\\roughSelectedButtonImage");
            _pressedImage = content.Load<Texture2D>("Content\\Textures\\roughPressedButtonImageDepressed");

            Unfocus();
            
        }
        
        public override void  UnloadGraphicsContent()
        {
            _baseImage = null;
            _focusedImage = null;
 	        _pressedImage = null;

            imageQuad.Image = null;
            
            base.UnloadGraphicsContent();
        }
        
       
         

        [needsXML]
        public void Focus()
        {
            imageQuad.Image = _focusedImage;
        }

        [needsXML]
        public void Unfocus()
        {
            imageQuad.Image = _baseImage;

        }

        [needsXML]
        public void Pressing()
        {
            imageQuad.Image = _pressedImage;
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
                        Unfocus();
                        break;
                }

                handled = true;
            }
            else if (guiEvent is GUISelectEvent)
            {
                GUISelectEvent selEvent = (GUISelectEvent)guiEvent;

                if (selEvent.SelectEvent == GUISelectEvent.GUISelectType.Accept)
                {
                    if (Accepted != null)
                    {
                        Accepted();
                    }
                }
                else
                {
                    if (guiEvent.GUIManager.IsFocused(this))
                    {
                        Focus();
                    }
                    else
                    {
                        Unfocus();
                    }

                }
            }

            return handled;
        }
        
    }
}
