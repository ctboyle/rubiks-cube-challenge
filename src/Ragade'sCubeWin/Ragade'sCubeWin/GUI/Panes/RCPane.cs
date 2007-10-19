using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.GraphicsManagement.BoundingVolumes;

namespace RagadesCubeWin.GUI.Panes
{
    class RCPane : RCFlatSpatial
    {
        private List<RCFlatSpatial> _listChildren;

        [needsXML]
        [placeHolder]
        public RCPane(
            float width,
            float height,
            int scrWidth,
            int scrHeight
            )
            : base(
                width, height,
                scrWidth, scrHeight
            )
        {
            _listChildren = new List<RCFlatSpatial>();
        }

        public override void Draw(GraphicsDevice graphicsDevice)
        {
            foreach (RCFlatSpatial child in _listChildren)
            {
                child.Draw(graphicsDevice);
            }
        }

        public override void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            )
        {
            foreach (RCFlatSpatial child in _listChildren)
            {
                child.LoadGraphicsContent(
                    graphics,
                    content
                    );
            }
        }

        protected override void UpdateWorldData(GameTime gameTime)
        {
            base.UpdateWorldData(gameTime);

            foreach (RCFlatSpatial child in _listChildren)
            {
                child.UpdateGS(gameTime, false);
            }
        }

        [needsXML]
        [placeHolder]
        public void AddChild(
            RCFlatSpatial newChild,
            long screenCoordX,
            long screenCoordY,
            float zOrder
            )
        {
            // Get percentage accross the pane window the screen coords are.
            float xScrPercent = (float)screenCoordX / (float)ScreenWidth;
            float yScrPercent = (float)screenCoordY / (float)ScreenHeight;

            float xLocalDist = xScrPercent * WorldWidth;
            float yLocalDist = -yScrPercent * WorldHeight;

            newChild.LocalTrans = Matrix.CreateTranslation(
                new Vector3(xLocalDist, yLocalDist, zOrder)
                );

            newChild.ResizeWorldSizeToPixelSize(
                WorldUnitsPerPixelWidth,
                WorldUnitsPerPixelHeight
                );

            newChild.parentNode = this;
            _listChildren.Add(newChild);

        }
    }
}
