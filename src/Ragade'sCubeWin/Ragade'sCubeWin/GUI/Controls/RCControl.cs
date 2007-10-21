using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.GUI.Panes;


namespace RagadesCubeWin.GUI
{
    [needsXML]
    internal abstract class RCControl : RCPane 
    {
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
        }

    }
}
