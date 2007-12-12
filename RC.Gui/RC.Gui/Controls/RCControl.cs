using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using RC.Engine.GraphicsManagement;
using RC.Gui.Panes;
using RC.Gui.Primitives;
using Microsoft.Xna.Framework.Graphics;


namespace RC.Gui
{
    [needsXML]
    public abstract class RCControl : RCPane
    {



        [placeHolder]
        [needsXML]
        internal RCControl(
            float width, 
            float height,
            int screenWidth, 
            int screenHeight
        ) : base(
            width,
            height,
            screenWidth,
            screenHeight
        )
        {

        }

       



        #region    ------------------------------Protected overridden functions
        
        [needsXML]
        public override void UnloadGraphicsContent()
        {
            //currentImageObject = null;
            base.UnloadGraphicsContent();
        }

        [needsXML]
        public override void Draw(GraphicsDevice graphicsDevice)
        {
            base.Draw(graphicsDevice);
        }

        [needsXML]
        protected override void UpdateWorldData(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.UpdateWorldData(gameTime);
        }

        #endregion ------------------------------Protected overridden functions
    }
}
