using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.GraphicsManagement.BoundingVolumes;

namespace RagadesCubeWin.GUI
{
    public abstract class  RCFlatSpatial : RCSpatial, IGUIElement
    {
        private int pixelWidth = -1;
        private int pixelHeight = -1;
        private int pixelPosX;
        private int pixelPosY;

        private float zOrder;

        private float boundingRectWidth;
        private float boundingRectHeight;
        
        private RCBoundingRect localBound;

        

        bool _acceptsFocus;

        public bool AcceptsFocus
        {
            get { return _acceptsFocus; }
            set { _acceptsFocus = value; }
        }

        public int XScreenPos
        {
            get { return pixelPosX; }
            set { pixelPosX = value; }
        }

        public int YScreenPos
        {
            get { return pixelPosY; }
            set { pixelPosY = value; }
        }

        public float ZOrder
        {
            get { return zOrder; }
            set { zOrder = value; }
        }

        public float WorldWidth
        {
            get { return boundingRectWidth; }
            set
            {
                boundingRectWidth = value;
                UpdateLocalBound();
            }
        }

        public float WorldHeight
        {
            get { return boundingRectHeight; }
            set
            {
                boundingRectHeight = value;
                UpdateLocalBound();
            }
        }

        public int ScreenWidth
        {
            get { return pixelWidth; }
            set { pixelWidth = value; }
        }

        public int ScreenHeight
        {
            get { return pixelHeight; }
            set { pixelHeight = value; }
        }

        public float WorldUnitsPerPixelWidth
        {
            get { return WorldWidth / ScreenWidth; }
        }

        public float PixelsPerWorldUnitWidth
        {
            get { return ScreenWidth / WorldWidth; }
        }

        public float WorldUnitsPerPixelHeight
        {
            get { return WorldHeight / ScreenHeight; }
        }

        public float PixelsPerWorldUnitHeight
        {
            get { return ScreenHeight / WorldHeight; }
        }
        
        public RCFlatSpatial(
            float width,
            float height,
            int scrWidth,
            int scrHeight
            )
            : base()
        {
            _acceptsFocus = false;

            // Create a trival local bound.
            localBound = new RCBoundingRect(
                Vector3.Zero,
                Vector3.Zero,
                Vector3.Zero
                );

            pixelWidth = scrWidth;
            pixelHeight = scrHeight;

            WorldHeight = height;
            WorldWidth = width;

            // Update local bound to reflect the pane dimentions
            UpdateLocalBound();
        }

        private void UpdateLocalBound()
        {
            // Create local bound to match width and height.

            // P1 - Upper left corner equals the origin
            localBound.P1 = Vector3.Zero;

            // P2 - Is width distance from p1 to the right;
            localBound.P2 = Vector3.Right * WorldWidth;

            // P3 - Is directly underneath P2 at height pixels
            localBound.P3 = localBound.P2 + Vector3.Down * WorldHeight;
        }

        public void ResizeWorldSizeToPixelSize(
            float worldToScreenX,
            float worldToScreenY
            )
        {
            WorldWidth = worldToScreenX * ScreenWidth;
            WorldHeight = worldToScreenY * ScreenHeight;
        }

        protected override void UpdateWorldBound()
        {
            _worldBound = localBound.Transform(_worldTrans);
        }

        #region IGUIElement Members

        public virtual bool OnEvent(GUIEvent guiEvent)
        {
            
            return false;
        }

        #endregion
    }
}

