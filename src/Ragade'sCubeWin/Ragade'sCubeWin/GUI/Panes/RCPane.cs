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
    class RCPane : RCNode
    {
        
        private int xLengthOnScreen = -1;
        private int yLengthOnScreen = -1;

        private float boundingRectWidth;
        private float boundingRectHeight;
        
        
        private RCBoundingRect localBound;

        //Should include a background.
        #region public read-write properties

        [placeHolder]
        [needsXML]
        public float WorldWidth
        {
            get { return boundingRectWidth; }
            set
            {
                boundingRectWidth = value;
                UpdateLocalBound();
            }
        }

        [placeHolder]
        [needsXML]
        public float WorldHeight
        {
            get {return boundingRectHeight;}
            set
            {
                boundingRectHeight = value;
                UpdateLocalBound();
            }
        }

        public int ScreenWidth
        {
            get {return xLengthOnScreen;}
            set{xLengthOnScreen = value;}
        }

        public int ScreenHeight
        {
            get{return yLengthOnScreen;}
            set{yLengthOnScreen = value;}
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

        #endregion public read-write properties


        public RCPane(
            float width,
            float height,
            int scrWidth,
            int scrHeight
            )
            : base()
        {

            /// Create a trival local bound.
            localBound = new RCBoundingRect(
                Vector3.Zero,
                Vector3.Zero,
                Vector3.Zero
                );

            xLengthOnScreen = scrWidth;
            yLengthOnScreen = scrHeight;

            WorldHeight = height;
            WorldWidth = width;

            // Update local bound to reflect the pane dimentions
            UpdateLocalBound();
        }

        // Makes the local bounding rect the size of the pane.
        private void UpdateLocalBound()
        {
            // P1 lives at origin
            localBound.P1 = Vector3.Zero;

            // P2 lives on x-axis at pane width.
            localBound.P2 = new Vector3(
                boundingRectWidth,
                0.0f,
                0.0f
                );
            
            // P3 lives below P2 at pane height.
            localBound.P3 = new Vector3(
                boundingRectWidth,
                -boundingRectHeight,
                0.0f
                );
            
        }

        #region ---Overridden functions from RCNode---

        [needsXML]
        [placeHolder]
        protected override void UpdateWorldBound()
        {
            _worldBound = localBound.Transform(_worldTrans);
        }

        


        #endregion ---Overridden functions from RCNode---

        [needsXML]
        [placeHolder]
        public void AddChild(
            RCSpatial newChild,
            long screenCoordX,
            long screenCoordY,
            float zOrder
            )
        {
            // TODO: Consider sizing child's width to match child's screen size.

            // Get percentage accross the pane window the screen coords are.
            float xScrPercent = (float)screenCoordX / (float)ScreenWidth;
            float yScrPercent = (float)screenCoordY / (float)ScreenHeight;

            float xLocalDist = xScrPercent * WorldWidth;
            float yLocalDist = -yScrPercent * WorldHeight;

            newChild.localTrans = Matrix.CreateTranslation(
                new Vector3(xLocalDist, yLocalDist, zOrder)
            );

            AddChild(newChild);
        }
    }
}
