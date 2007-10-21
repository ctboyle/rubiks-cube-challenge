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

        /// <summary>
        /// The private RCQuad object that is being displayed at the current time.
        /// </summary>
        private RCQuad currentImageObject = null;

        [needsXML]
        private RCQuad baseImageObject = null;

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
            

            baseImageObject = new RCQuad(width, height, screenWidth, screenHeight);
            //            AddChild(selectedImageObject, 0, 0, 0f);
            
            selectedImageObject = new RCQuad(width, height, screenWidth, screenHeight);
//            AddChild(selectedImageObject, 0, 0, 0f);

            activatingImageObject = new RCQuad(width, height, screenWidth, screenHeight);
//            AddChild(activatingImageObject, 0, 0, 0f);


            currentImageObject = baseImageObject;
            AddChild(currentImageObject, 0, 0, 0f);

        }

        #region    ------------------------------Overridden functions

        [placeHolder]
        [needsXML]
        public override void Draw(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            base.Draw(graphicsDevice);
        }

        [needsXML]
        protected override void UpdateWorldData(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.UpdateWorldData(gameTime);
        }

        [needsXML]
        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadGraphicsContent(graphics, content);
            baseImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughButtonImage");
            selectedImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughSelectedButtonImage");
            activatingImageObject.Image = content.Load<Texture2D>("Content\\Textures\\roughPressedButtonImageDepressed");
            
        }
        
        
       
         

        #endregion ------------------------------Overridden functions

        [needsXML]
        public void Select()
        {
            currentImageObject = selectedImageObject;
        }

        [needsXML]
        public void Deselect()
        {
            currentImageObject = baseImageObject;
        }

        [needsXML]
        public void Pressing()
        {
            currentImageObject = activatingImageObject;
        }






    }
}
