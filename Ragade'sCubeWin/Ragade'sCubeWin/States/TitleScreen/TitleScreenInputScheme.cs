using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Types;




namespace RagadesCubeWin.States.TitleScreen
{
    class TitleScreenInputScheme : RCInputScheme<RCTitleScreenState>
    {
        public TitleScreenInputScheme()
            :base()
        {
        }


        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();
            MouseWatcher mouseWatcher = new MouseWatcher();
            XBox360GamePad gamePadWatcher = new XBox360GamePad(PlayerIndex.One);

            List<IWatcher> mappedWatchers = new List<IWatcher>();

            if (mouseWatcher.DetectMyInput())
            {
                mouseWatcher.WatchEvent(new MouseEvent(
                    MouseInput.LeftButton,
                    EventTypes.OnDown,
                    delegate(Vector2 pos, Vector2 move)
                    {
                        ControlItem.StartLeaveAnimation();
                    }
                    ));

                mappedWatchers.Add(mouseWatcher);
            }

            if (keyWatcher.DetectMyInput())
            {

                keyWatcher.WatchEvent(new KeyboardEvent(
                    EventTypes.OnDown,
                    delegate(Keys key)
                    {
                        ControlItem.StartLeaveAnimation();
                    }
                    ));

                mappedWatchers.Add(keyWatcher);
            }

            if (gamePadWatcher.DetectMyInput())
            {
                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.A,
                   EventTypes.OnDown,
                   ControlItem.StartLeaveAnimation
                   ));

                

                mappedWatchers.Add(gamePadWatcher);
            }

            return mappedWatchers.ToArray();
        }
    }
}
