using System;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using Microsoft.Xna.Framework.Input;
using RagadesCubeWin.Input.Types;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.Input.Events;

namespace RagadesCubeWin.GameLogic.InputSchemes
{
    public class RCGLKeyboardInputScheme : RCGLInputScheme
    {
        public Keys LeftPressKey = Keys.A;
        public Keys RightPressKey = Keys.D;
        public Keys UpPressKey = Keys.W;
        public Keys DownPressKey = Keys.S;

        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();

            #region Move

            KeyboardEvent moveLeft = new KeyboardEvent(
                Keys.LeftShift,
                EventTypes.Pressed,
                OnLeftMove
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    LeftPressKey,
                    EventTypes.Pressed,
                    moveLeft
                )
            );

            KeyboardEvent moveRight = new KeyboardEvent(
                Keys.LeftShift,
                EventTypes.Pressed,
                OnRightMove
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    RightPressKey,
                    EventTypes.Pressed,
                    moveRight
                )
            );

            KeyboardEvent moveUp = new KeyboardEvent(
                Keys.LeftShift,
                EventTypes.Pressed,
                OnUpMove
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    UpPressKey,
                    EventTypes.Pressed,
                    moveUp
                )
            );

            KeyboardEvent moveDown = new KeyboardEvent(
                Keys.LeftShift,
                EventTypes.Pressed,
                OnDownMove
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    DownPressKey,
                    EventTypes.Pressed,
                    moveDown
                )
            );

            #endregion

            #region Cursor Move

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    LeftPressKey,
                    EventTypes.Pressed,
                    OnLeftCursorMove
                )
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    RightPressKey,
                    EventTypes.Pressed,
                    OnRightCursorMove
                )
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    UpPressKey,
                    EventTypes.Pressed,
                    OnUpCursorMove
                )
            );

            keyWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.KeyboardEvent(
                    DownPressKey,
                    EventTypes.Pressed,
                    OnDownCursorMove
                )
            );

            #endregion

            return new IWatcher[] { keyWatcher };
        }

        private void OnLeftCursorMove()
        {
            MoveCursor(new Vector2(-1, 0));
        }

        private void OnRightCursorMove()
        {
            MoveCursor(new Vector2(1, 0));
        }

        private void OnUpCursorMove()
        {
            MoveCursor(new Vector2(0, 1));
        }

        private void OnDownCursorMove()
        {
            MoveCursor(new Vector2(0, -1));
        }

        private void OnLeftMove()
        {
            Move(new Vector2(0, -0.05f));
        }

        private void OnRightMove()
        {
            Move(new Vector2(0, 0.05f));
        }

        private void OnUpMove()
        {
            Move(new Vector2(-0.05f, 0));
        }

        private void OnDownMove()
        {
            Move(new Vector2(0.05f, 0));
        }
    }
}
