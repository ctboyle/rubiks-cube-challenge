using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.States.MainMenu;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GUI;
using RagadesCubeWin.GUI.Fonts;


namespace RagadesCubeWin.States.MenuCubeState.CubeMenus
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
            options.buttonText.Text = "options";
            options.buttonText.CenterText = true;
            options.AfterPressedAndReleased +=
                delegate()
                {
                    gameManager.PushState(new RCOptions(Game));
                };

            
            _menuPane.AddChild(options, 0, 300, 0.0f);

        }

        
    }
}
