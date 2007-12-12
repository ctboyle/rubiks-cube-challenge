using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using RC.Engine.GraphicsManagement;

namespace RC.Gui
{
    public interface IGUIElement: ISpatial
    {
        bool OnEvent(GUIEvent guiEvent);
        bool AcceptsFocus { get; set;}
    }
}
