using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCube.States;
using RagadesCube.Controllers;
using RC.Gui;
using RC.Gui.Fonts;
using RC.Gui.Primitives;


namespace RagadesCube.States
{
    class RCExit: RCCubeMenu
    {

        public RCExit(Game game)
            : base(game)
        {
            _menuPos = RCMenuCameraController.CameraPositions.Bottom;
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void ConstructGuiElements()
        {
            _titleText.Text = "Exit";

            BitmapFont mediumFont = _fontManager.GetFont("Ragade's Cube Medium");

            // Prompt
            RCText promptText = new RCText(mediumFont, 1, 1, 600, 75);
            promptText.Text = "Are you sure?";
            promptText.CenterText = true;
            _menuPane.AddChild(promptText, 0, 250, 0.0f);


            // New game button.
            RCButton yesButton = new RCButton(1, 1, 200, 75, mediumFont);
            yesButton.buttonText.Text = "Exit";
            yesButton.buttonText.CenterText = true;
            yesButton.AfterPressedAndReleased +=
                delegate()
                {
                    Game.Exit();
                };

            _menuPane.AddChild(yesButton, 80, 325, 0.0f);

            RCButton noButton = new RCButton(1, 1, 200, 75, mediumFont);
            noButton.buttonText.Text = "Return";
            noButton.buttonText.CenterText = true;
            noButton.AfterPressedAndReleased +=
                delegate()
                {
                    gameManager.PopState();
                };


            _menuPane.AddChild(noButton, 320, 325, 0.0f);

        }
    }
}