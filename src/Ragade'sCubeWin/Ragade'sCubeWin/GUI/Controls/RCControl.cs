using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.GUI.Primitives;
using Microsoft.Xna.Framework.Graphics;


namespace RagadesCubeWin.GUI
{
    [needsXML]
    internal abstract class RCControl : RCPane
    {
        #region    ------------------------------Protected data members
        
        [scaffolding]
        /// <summary>
        /// The protected RCQuad object that is being displayed at the current time.
        /// </summary>
        protected RCQuad currentImageObject = null;

        /// <summary>
        /// The protected RCQuad object that represents the object in question with absolutely no modification.
        /// </summary>
        protected RCQuad baseImageObject = null;

        [needsXML]
        protected List<RCQuad> visibleQuads = new List<RCQuad>();
        #endregion ------------------------------Protected  data members

        #region    ------------------------------Protected virtual functions
        [needsXML]
        virtual protected void instantiateBaseAndCurrentImageObjects(float width, float height, int screenWidth, int screenHeight)
        {
            currentImageObject = new RCQuad(width, height, screenWidth, screenHeight);
            baseImageObject = new RCQuad(width, height, screenWidth, screenHeight);
        }
        #endregion ------------------------------Protected virtual functions


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
            currentImageObject = new RCQuad(width, height, screenWidth, screenHeight);
            baseImageObject = new RCQuad(width, height, screenWidth, screenHeight);
        }

       



        #region    ------------------------------Protected overridden functions
        
        [needsXML]
        public override void UnloadGraphicsContent()
        {
            currentImageObject = null;
            baseImageObject = null;
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
