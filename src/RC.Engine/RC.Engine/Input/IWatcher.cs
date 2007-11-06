using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RC.Engine.Input
{
    /// <summary>
    /// Watcher Interface
    ///     - just has the necessary functions guranteed that all
    ///     - watchers will have. 
    /// </summary>
    public interface IWatcher
    {
        bool DetectMyInput();
        void RunEvents();
    }
}
