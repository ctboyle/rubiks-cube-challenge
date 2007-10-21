using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
