using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RagadesCube.States;
using RC.Input;
using RC.Engine.Input;
using RC.Engine.StateManagement;
using RC.Input.Types;
using RC.Input.Watchers;
using RC.Input.Events;


namespace RagadesCube.States.InputSchemes
{
    class CubeMenuInputScheme : RCInputScheme<RCCubeMenu>
    {
        IGameStateManager _stateManager;
        public CubeMenuInputScheme()
            :base()
        {

        }


        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();
       
            XBox360GamePad gamePadWatcher = new XBox360GamePad(PlayerIndex.One);

            List<IWatcher> mappedWatchers = new List<IWatcher>();

            if (keyWatcher.DetectMyInput())
            {

                keyWatcher.WatchEvent(new KeyboardEvent(
                    Keys.Escape,
                    EventTypes.OnDown,
                    ControlItem.ExitState
                    ));

                mappedWatchers.Add(keyWatcher);
            }

            if (gamePadWatcher.DetectMyInput())
            {
                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.B,
                   EventTypes.Pressed,
                   ControlItem.ExitState
                   ));

                

                mappedWatchers.Add(gamePadWatcher);
            }

            return mappedWatchers.ToArray();
        }
    }
}
