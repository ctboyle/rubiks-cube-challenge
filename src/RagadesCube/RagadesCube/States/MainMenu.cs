using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCube.States;
using RC.Gui;
using RC.Gui.Fonts;




namespace RagadesCube.States
{
    class RCMainMenu : RCCubeMenu
    {
        public RCMainMenu(Game game)
            :base(game)
        {
                   
        }


        public override void Initialize()
        {
            base.Initialize();
            _titleText.Text = "Main Menu";

            
        }

        protected override void ConstructGuiElements()
        {
            BitmapFont mediumFont = _fontManager.GetFont("Ragade's Cube Medium");
            mediumFont.KernEnable = false;
            // New game button.
            RCButton newGame = new RCButton(1, 1, 600, 75, mediumFont);
            newGame.buttonText.Text = "New Game";
            newGame.buttonText.CenterText = true;
            newGame.AfterPressedAndReleased +=
                delegate()
                {
                    gameManager.PushState(new RCNewGame(Game));
                };

            _menuPane.AddChild(newGame, 0, 225, 0.0f);

            RCButton options = new RCButton(1, 1, 600, 75, mediumFont);
            options.buttonText.Text = "Credits";
            options.buttonText.CenterText = true;
            options.AfterPressedAndReleased +=
                delegate()
                {
                    gameManager.PushState(new RCOptions(Game));
                };

            
            _menuPane.AddChild(options, 0, 300, 0.0f);

            RCButton exit = new RCButton(1, 1, 600, 75, mediumFont);
            exit.buttonText.Text = "Exit";
            exit.buttonText.CenterText = true;
            exit.AfterPressedAndReleased +=
                delegate()
                {
                    gameManager.PushState(new RCExit(Game));
                };


            _menuPane.AddChild(exit, 0, 375, 0.0f);

        }

        
    }
}
