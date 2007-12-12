using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.States.MainMenu;


namespace RagadesCubeWin.States.MenuCubeState.CubeMenus
{
     class RCOptions : RCCubeMenu
    {

        public RCOptions(Game game)
            : base(game)
        {
            _menuPos = RCMenuCameraController.CameraPositions.Left;
        }


        public override void Initialize()
        {
            base.Initialize();   
        }

        protected override void ConstructGuiElements()
        {
            _titleText.Text = "Options";
        }
    }
}

