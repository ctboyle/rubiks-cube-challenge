using System;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Types;
using Microsoft.Xna.Framework;
using RagadesCubeWin.SceneObjects;

namespace RagadesCubeWin.GameLogic.InputSchemes
{
    public class RCGLMouseInputScheme : RCGLInputScheme
    {
        bool _clickActive = false;
        bool _isMoving = true;

        protected override IWatcher[] MapWatcherEvents()
        {
            MouseWatcher mouseWatcher = new MouseWatcher();

            mouseWatcher.WatchEvent(
               new RagadesCubeWin.Input.Events.MouseEvent(
                   MouseInput.RightButton,
                   EventTypes.OnDown,
                   delegate(Vector2 position, Vector2 move)
                   {
                       Rotate(RCCube.RotationDirection.Clockwise);
                   }
               )
           );

            mouseWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.MouseEvent(
                    MouseInput.LeftButton,
                    EventTypes.OnUp,
                    delegate(Vector2 position, Vector2 move)
                    {
                        if (!_isMoving)
                            Rotate(RCCube.RotationDirection.CounterClockwise);

                        _clickActive = false;
                        _isMoving = false;
                    }
                )
            );

            mouseWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.MouseEvent(
                    MouseInput.LeftButton,
                    EventTypes.OnDown,
                    delegate(Vector2 position, Vector2 move)
                    {
                        _clickActive = true;
                    }
                )
            );

            mouseWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.MouseEvent(
                    MouseInput.NoButton,
                    EventTypes.Leaned,
                    delegate(Vector2 position, Vector2 move)
                    {
                        if (move == Vector2.Zero) return;

                        if (_clickActive)
                        {
                            Move(new Vector2(-move.Y, -move.X) / 100);
                            _isMoving = true;
                        }
                        else
                        {
                            position.X -= Player.Camera.Viewport.X;
                            position.Y -= Player.Camera.Viewport.Y;

                            position.X -= (Player.Camera.Viewport.Width / 2);
                            position.Y -= (Player.Camera.Viewport.Height / 2);

                            position.Y = -position.Y;

                            position.Normalize();
                            MoveCursor(position);
                        }
                    }
                )
            );

            return new IWatcher[] { mouseWatcher };
        }
    }
}
