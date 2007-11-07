using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.StateManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.States.MainMenu;



namespace RagadesCubeWin.States.MenuCubeState
{
    class RCMenuCubeState : RCGameState
    {
        static protected RCMenuCubeScene _menuScene;

        public RCMenuCubeState(
            Game game
            )
            : base(game)
        {
            if (_menuScene == null)
            {
                _menuScene = new RCMenuCubeScene(graphics.GraphicsDevice.Viewport);
            } 
        }

        public override void Initialize()
        {
            _sceneManager.AddScene(_menuScene);

            base.Initialize();
        }
    }
}
