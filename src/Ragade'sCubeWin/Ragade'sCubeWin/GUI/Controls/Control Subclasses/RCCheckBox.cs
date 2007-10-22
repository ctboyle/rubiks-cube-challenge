using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RagadesCubeWin.GUI
{
    [placeHolder]
    [needsXML]
    class RCCheckBox : RCControl
    {
        [placeHolder]
        [needsXML]
        internal RCCheckBox(
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


        [needsXML]
        [placeHolder]
        protected override void instantiateBaseAndCurrentImageObjects(float width, float height, int screenWidth, int screenHeight)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
