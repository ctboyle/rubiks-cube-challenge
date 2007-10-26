using System;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Types;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GameLogic.InputSchemes
{
    public class RCGLMouseInputScheme : RCGLInputScheme
    {
        protected override IWatcher[] MapWatcherEvents()
        {
            MouseWatcher mouseWatcher = new MouseWatcher();

            mouseWatcher.WatchEvent(
                new RagadesCubeWin.Input.Events.MouseEvent(
                    MouseInput.NoButton,
                    EventTypes.Leaned, 
                    OnMove
                )
            );

            return new IWatcher[] { mouseWatcher };
        }

        private void OnMove(Vector2 position, Vector2 move)
        {
            Move(move / 1000);
        }
    }
}
