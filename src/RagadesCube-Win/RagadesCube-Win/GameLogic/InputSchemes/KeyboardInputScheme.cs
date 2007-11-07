using System;
using RC.Engine.Input;
using RC.Input.Watchers;
using Microsoft.Xna.Framework.Input;
using RC.Input.Types;
using Microsoft.Xna.Framework;
using RC.Engine.Cameras;
using RC.Input.Events;
using RagadesCube.SceneObjects;

namespace RagadesCube.GameLogic.InputSchemes
{
    public class RCGLKeyboardInputScheme : RCGLInputScheme
    {
        public Keys LeftPressKey = Keys.A;
        public Keys RightPressKey = Keys.D;
        public Keys UpPressKey = Keys.W;
        public Keys DownPressKey = Keys.S;
        public Keys FirePressKey = Keys.F;
        public Keys ExitKey = Keys.Escape;

        bool _leftShiftActive = false;

        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    Keys.LeftShift,
                    EventTypes.OnDown,
                    delegate()
                    {
                        _leftShiftActive = true;
                    }
                )
            );

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    Keys.LeftShift,
                    EventTypes.OnUp,
                    delegate()
                    {
                        _leftShiftActive = false;
                    }
                )
            );

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    ExitKey,
                    EventTypes.Pressed,
                    delegate()
                    {
                        ControlItem.StopGame();
                    }
                )
            );

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    LeftPressKey,
                    EventTypes.Pressed,
                    delegate()
                    {
                        if(_leftShiftActive)
                            Move(new Vector2(0, -0.05f));
                        else
                            MoveCursor(new Vector2(-1, 0));
                    }
                )
            );

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    RightPressKey,
                    EventTypes.Pressed,
                    delegate()
                    {
                        if (_leftShiftActive)
                            Move(new Vector2(0, 0.05f));
                        else
                            MoveCursor(new Vector2(1, 0));
                    }
                )
            );

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    UpPressKey,
                    EventTypes.Pressed,
                    delegate()
                    {
                        if (_leftShiftActive)
                            Move(new Vector2(-0.05f, 0));
                        else
                            MoveCursor(new Vector2(0, 1));
                    }
                )
            );

            keyWatcher.WatchEvent(
                new RC.Input.Events.KeyboardEvent(
                    DownPressKey,
                    EventTypes.Pressed,
                    delegate()
                    {
                        if (_leftShiftActive)
                            Move(new Vector2(0.05f, 0));
                        else
                            MoveCursor(new Vector2(0, -1));
                    }
                )
            );

            keyWatcher.WatchEvent(
                new KeyboardEvent(
                    FirePressKey,
                    EventTypes.Pressed,
                    delegate()
                    {
                        if(_leftShiftActive)
                            Rotate(RCCube.RotationDirection.Clockwise);
                        else
                            Rotate(RCCube.RotationDirection.CounterClockwise);
                    }
                )
            );

            return new IWatcher[] { keyWatcher };
        }
    }
}
