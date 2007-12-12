using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using RagadesCubeWin.GraphicsManagement;

namespace RagadesCubeWin.GUI
{
    public interface IGUIElement: ISpatial
    {
        bool OnEvent(GUIEvent guiEvent);
        bool AcceptsFocus { get; set;}
    }
}
