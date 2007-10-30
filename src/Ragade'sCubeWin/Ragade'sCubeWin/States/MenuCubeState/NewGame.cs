using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.States.MainMenu;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.States.MenuCubeState.CubeMenus
{
    class RCNewGame : RCCubeMenu
    {

        public RCNewGame(Game game)
            : base(game)
        {
            _menuPos = RCMenuCameraController.CameraPositions.Right;
        }


        public override void Initialize()
        {
            base.Initialize();   
        }

        protected override void ConstructGuiElements()
        {
            _titleText.Text = "New Game";
        }
    }
}
