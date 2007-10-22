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

        protected override IWatcher[] MapWatcherEvents(RCTestState cntrlItem)
        {
            List<IWatcher> watchers = new List<IWatcher>();

            #region GamePadWatcher

            XBox360GamePad watchplayer1 = new XBox360GamePad(PlayerIndex.One);

            if (watchplayer1.DetectMyInput())
            {
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DLEFT, EventTypes.OnDown, cntrlItem.OnSelHorizontalFace));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DDOWN, EventTypes.OnDown, cntrlItem.OnSelVerticalFace));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DRIGHT, EventTypes.OnDown, cntrlItem.OnSelOppFace));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.LEFTSHOULDER, EventTypes.OnDown, cntrlItem.OnRotateUp));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.RIGHTSHOULDER, EventTypes.OnDown, cntrlItem.OnRotateDown));
                watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.LEFTANALOG, EventTypes.Leaned, cntrlItem.CubeMove));
                watchers.Add(watchplayer1);
            }

            #endregion

            #region Keyboardwatcher

            KeyboardWatcher watchkeyboard = new KeyboardWatcher();

            if (watchkeyboard.DetectMyInput())
            {
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, cntrlItem.YRotUp));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.D, EventTypes.Pressed, cntrlItem.YRotDown));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.W, EventTypes.Pressed, cntrlItem.XRotUp));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.S, EventTypes.Pressed, cntrlItem.XRotDown));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageUp, EventTypes.Pressed, cntrlItem.OnRotateUp));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageDown, EventTypes.Pressed, cntrlItem.OnRotateDown));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Up, EventTypes.OnUp, cntrlItem.OnSelHorizontalFace));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Down, EventTypes.OnUp, cntrlItem.OnSelVerticalFace));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Left, EventTypes.OnUp, cntrlItem.OnSelOppFace));
                watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Right, EventTypes.OnUp, cntrlItem.OnSelOppFace));
                watchers.Add(watchkeyboard);
            }

            #endregion

            return watchers.ToArray();
        }
    }
}
