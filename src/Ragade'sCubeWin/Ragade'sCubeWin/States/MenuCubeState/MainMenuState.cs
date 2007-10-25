using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.StateManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.States.MainMenu;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.States
{
    class RCMainMenuState : RCGameState
    {
        
        RCMainMenuScene menuScene;
        RCMainMenuSceneInputScheme menuInputScheme;

        public RCMainMenuState(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {            
            menuScene = new RCMainMenuScene(graphics.GraphicsDevice.Viewport);
            _sceneManager.AddScene(menuScene);

            menuInputScheme = new RCMainMenuSceneInputScheme(input);
            menuInputScheme.Apply(menuScene);
            base.Initialize();
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            

            base.LoadGraphicsContent(loadAllContent);
        }
    }
}
