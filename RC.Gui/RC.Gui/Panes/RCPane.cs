using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using RC.Engine.GraphicsManagement;
using RC.Engine.GraphicsManagement.BoundingVolumes;

namespace RC.Gui.Panes
{
    public class RCPane : RCFlatSpatial, INode
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
                ResizeChild(child);
                child.UpdateGS(gameTime, false);
            }
        }

        [needsXML]
        [placeHolder]
        public virtual void AddChild(
            RCFlatSpatial newChild,
            int screenCoordX,
            int screenCoordY,
            float zOrder
            )
        {
            newChild.XScreenPos = screenCoordX;
            newChild.YScreenPos = screenCoordY;
            newChild.ZOrder = zOrder;

            ResizeChild(newChild);

            newChild.parentNode = this;
            _listChildren.Add(newChild);

        }

        private void ResizeChild(RCFlatSpatial newChild)
        {

            // Get percentage accross the pane window the screen coords are.
            float xScrPercent = (float)newChild.XScreenPos / (float)ScreenWidth;
            float yScrPercent = (float)newChild.YScreenPos / (float)ScreenHeight;

            float xLocalDist = xScrPercent * WorldWidth;
            float yLocalDist = -yScrPercent * WorldHeight;

            newChild.LocalTrans = Matrix.CreateTranslation(
                new Vector3(xLocalDist, yLocalDist, newChild.ZOrder)
                );

            newChild.ResizeWorldSizeToPixelSize(
                WorldUnitsPerPixelWidth,
                WorldUnitsPerPixelHeight
                );
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

        #region INode Members

        public List<ISpatial> GetChildren()
        {
            List<ISpatial> listSpatials = new List<ISpatial>(
                _listChildren.Count
                );

            foreach (RCFlatSpatial child in _listChildren)
            {
                listSpatials.Add(child);
            }

            return listSpatials;
        }

        #endregion
    }
}
