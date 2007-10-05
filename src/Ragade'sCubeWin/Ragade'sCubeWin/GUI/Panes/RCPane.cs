using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GraphicsManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Ragades_Cube_2D.Classes.Panes
{
    class RCPane : RCNode, RCIGUIObject
    {
        
        private int xLengthOnScreen = -1;
        private int yLengthOnScreen = -1;
        //Should include a background.


        #region public read-write properties
        [placeHolder]
        [needsXML]
        public int worldWidth
        {
            get
            {
            }
            set
            {
            }
        }

        [placeHolder]
        [needsXML]
        public int worldWidth
        {
            get
            {
            }
            set
            {
            }
        }
        #endregion public read-write properties

        #region ---------------------------------------Overridden functions from RCNode---------------------------------------

        [needsXML]
        [placeHolder]
        protected override void UpdateWorldBound()
        {
            base.UpdateWorldBound();
        }

        


        #endregion ---------------------------------------Overridden functions from RCNode---------------------------------------

        [needsXML]
        [placeHolder]
        public void addChild(RCPane newChild, long screenCoordX, long screenCoordY, float zOrder)
        {
            
            listChildren.Add(newChild);
        }




        
    }
}
