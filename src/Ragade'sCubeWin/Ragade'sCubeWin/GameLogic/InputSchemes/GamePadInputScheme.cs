using System;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input.Types;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.SceneObjects;

namespace RagadesCubeWin.GameLogic.InputSchemes
{
    public class RCGLGamePadInputScheme : RCGLInputScheme
    {
        private PlayerIndex _playerIndex;
        private bool _isTriggerPressed;

        public RCGLGamePadInputScheme(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
        }

        protected override IWatcher[] MapWatcherEvents()
        {
            XBox360GamePad gamePad = new XBox360GamePad(_playerIndex);

            if (!gamePad.DetectMyInput())
               //throw new Exception("Unable to map player input because the input device cannot be detected.");

            gamePad.WatchEvent(
                 new XBox360GamePadEvent(
                     XBox360GamePadTypes.START,
                     EventTypes.Pressed,
                     OnExit
                 )
             );

            gamePad.WatchEvent(
                 new XBox360GamePadEvent(
                     XBox360GamePadTypes.LEFTTRIGGER,
                     EventTypes.Leaned,
                     OnTrigger
                 )
             );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.LEFTANALOG, 
                    EventTypes.Leaned, 
                    OnLeftStick
                )
            );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.LEFTSHOULDER,
                    EventTypes.Pressed,
                    OnLeftButton
                )
            );

            gamePad.WatchEvent(
                new XBox360GamePadEvent(
                    XBox360GamePadTypes.RIGHTSHOULDER,
                    EventTypes.Pressed,
                    OnRightButton
                )
            );

            return new IWatcher[] { gamePad };
        }

        private void OnExit()
        {
            ControlItem.Stop();
        }

        private void OnLeftButton()
        {
            Rotate(RCCube.RotationDirection.CounterClockwise);
        }

        private void OnRightButton()
        {
            Rotate(RCCube.RotationDirection.Clockwise);
        }

        private void OnTrigger(Vector2 position, Vector2 move)
        {
            _isTriggerPressed = (position.Length() > 0);
        }

        private void OnLeftStick(Vector2 position, Vector2 move)
        {
            const float stickThreshold = 0.50f;

            if (position.Length() < Math.Abs(stickThreshold))
                return;

            if (_isTriggerPressed)
                Move(new Vector2(-position.Y, position.X) / 20);
            else
                MoveCursor(position);
        }
    }
}