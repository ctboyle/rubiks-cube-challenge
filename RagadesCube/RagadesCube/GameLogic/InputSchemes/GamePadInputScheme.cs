using System;
using RC.Engine.Input;
using RC.Input.Watchers;
using Microsoft.Xna.Framework;
using RC.Input.Events;
using RC.Input.Types;
using RC.Engine.Cameras;
using RagadesCube.SceneObjects;

namespace RagadesCube.GameLogic.InputSchemes
{
    public class RCGLGamePadInputScheme : RCGLInputScheme
    {
        private PlayerIndex _playerIndex;
        private bool _isTriggerPressed;
        private bool _rightTriggerPressed;

        public RCGLGamePadInputScheme(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
        }

        protected override IWatcher[] MapWatcherEvents()
        {
            XBox360GamePad gamePad = new XBox360GamePad(_playerIndex);

            if (!gamePad.DetectMyInput())
               throw new Exception("Unable to map player input because the input device cannot be detected.");

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.START,
                    EventTypes.Pressed,
                    delegate()
                    {
                        ControlItem.StopGame();
                    }
                )
            );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.A,
                    EventTypes.OnDown,
                    delegate()
                    {
                        Orient();
                    }
                )
            );

            gamePad.WatchEvent(
                 new XBox360GamePadEvent(
                     XBox360GamePadTypes.LEFTTRIGGER,
                     EventTypes.Leaned,
                     delegate(Vector2 position, Vector2 move)
                     {
                         _isTriggerPressed = (position.Length() > 0);
                     }
                 )
             );

            gamePad.WatchEvent(
                 new XBox360GamePadEvent(
                     XBox360GamePadTypes.RIGHTTRIGGER,
                     EventTypes.Leaned,
                     delegate(Vector2 position, Vector2 move)
                     {
                         if (position.Length() > 0 )
                         {
                             if (_rightTriggerPressed == false)
                             {
                                _rightTriggerPressed = true;
                                Orient();
                             }
                         }
                         else
                         {
                             _rightTriggerPressed = false;
                         }

                     }
                 )
             );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.LEFTANALOG,
                    EventTypes.Leaned,
                    delegate(Vector2 position, Vector2 move)
                    {
                        // a threshold of 0.15 is used so that
                        // every lean event won't get processed.
                        if (position.Length() < 0.15f) return;

                        if (_isTriggerPressed)
                        {
                            Move(new Vector2(-position.Y, position.X) * MathHelper.PiOver2);
                        }
                        else
                            MoveCursor(position);
                    }
                )
            );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.LEFTSHOULDER,
                    EventTypes.Pressed,
                    delegate()
                    {
                        Rotate(RCCube.RotationDirection.CounterClockwise);
                    }
                )
            );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.RIGHTSHOULDER,
                    EventTypes.Pressed,
                    delegate()
                    {
                        Rotate(RCCube.RotationDirection.Clockwise);
                    }
                )
            );

            return new IWatcher[] { gamePad };
        }
    }
}