using System;
using System.Collections.Generic;
using System.Text;

namespace RagadesCubeWin.GUI
{
    [placeHolder]
    [needsXML]
    class RCLabel : RCControl
    {
        [placeHolder]
        [needsXML]
        internal RCLabel(
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
    }
}
