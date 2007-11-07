using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCube.States;
using RagadesCube.Controllers;


namespace RagadesCube.States
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

