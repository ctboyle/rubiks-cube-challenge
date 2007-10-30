using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.States.MenuCubeState.CubeMenus;
using RagadesCubeWin.Input;
using RagadesCubeWin.StateManagement;
using Microsoft.Xna.Framework.Input;
using RagadesCubeWin.Input.Types;
using RagadesCubeWin.Input.Watchers;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Input.Events;

namespace RagadesCubeWin.States.MenuCubeState
{
    class CubeMenuInputScheme : RCInputScheme<RCCubeMenu>
    {
        RCGameStateManager _stateManager;
        public CubeMenuInputScheme(InputManager im)
            :base(im)
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
