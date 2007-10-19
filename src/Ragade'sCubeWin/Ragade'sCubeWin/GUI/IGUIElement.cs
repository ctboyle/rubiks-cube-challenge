using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;


namespace RagadesCubeWin.GUI
{
    interface IGUIElement
    {
        bool OnEvent(GUIEvent guiEvent);

    }
}
