using System;
using System.Collections.Generic;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Types;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Input.Events;

namespace RagadesCubeWin.States
{
    public class RCTestInputScheme : RCInputScheme<RCTestState>
    {
        public RCTestInputScheme(InputManager inMgr)
            : base(inMgr)
        {
        }

        protected override IWatcher[] MapWatcherEvents()
        {
            List<IWatcher> watchers = new List<IWatcher>();

            #region GamePadWatcher

            XBox360GamePad watchplayer1 = new XBox360GamePad(PlayerIndex.One);

            if (watchplayer1.DetectMyInput())
            {
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DLEFT, EventTypes.OnDown, ControlItem.OnSelHorizontalFace));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DDOWN, EventTypes.OnDown, ControlItem.OnSelVerticalFace));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DRIGHT, EventTypes.OnDown, ControlItem.OnSelOppFace));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.LEFTSHOULDER, EventTypes.OnDown, ControlItem.OnRotateUp));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.RIGHTSHOULDER, EventTypes.OnDown, ControlItem.OnRotateDown));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.LEFTANALOG, EventTypes.Leaned, ControlItem.CubeMove));
                watchers.Add(watchplayer1);
            }

            #endregion

            #region Keyboardwatcher

            KeyboardWatcher watchkeyboard = new KeyboardWatcher();

            if (watchkeyboard.DetectMyInput())
            {
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, ControlItem.YRotUp));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.D, EventTypes.Pressed, ControlItem.YRotDown));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.W, EventTypes.Pressed, ControlItem.XRotUp));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.S, EventTypes.Pressed, ControlItem.XRotDown));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageUp, EventTypes.Pressed, ControlItem.OnRotateUp));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageDown, EventTypes.Pressed, ControlItem.OnRotateDown));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Up, EventTypes.OnUp, ControlItem.OnSelHorizontalFace));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Down, EventTypes.OnUp, ControlItem.OnSelVerticalFace));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Left, EventTypes.OnUp, ControlItem.OnSelOppFace));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Right, EventTypes.OnUp, ControlItem.OnSelOppFace));
                watchers.Add(watchkeyboard);
            }

            #endregion

            return watchers.ToArray();
        }
    }
}
