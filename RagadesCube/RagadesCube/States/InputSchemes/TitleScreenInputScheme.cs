using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RC.Input;
using RC.Input.Watchers;
using RC.Input.Events;
using RC.Engine.Input;
using RC.Input.Types;




namespace RagadesCube.States.InputSchemes
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
            XBox360GamePad gamePadWatcher = new XBox360GamePad(PlayerIndex.One);

#if !XBOX
            MouseWatcher mouseWatcher = new MouseWatcher();
#endif

            List<IWatcher> mappedWatchers = new List<IWatcher>();

#if !XBOX

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

#endif

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
