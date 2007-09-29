using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input
{
    public interface IWatcher
    {
        bool DetectMyInput();
        bool WatchEvent(Input.Events.Event e);
        void RunEvents();
    }
}
