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
    public class RCPane : RCFlatSpatial
    {
        protected List<RCFlatSpatial> _listChildren;

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

        public override void UnloadGraphicsContent()
        {
            foreach (RCFlatSpatial child in _listChildren)
            {
                child.UnloadGraphicsContent();
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

        [needsXML]
        [placeHolder]
        public bool RemoveChild(RCFlatSpatial removeChild)
        {
            bool removed = false;
            if (removeChild != null)
            {
                removed = _listChildren.Remove(removeChild);
            }
            return removed;
        }
    }
}
