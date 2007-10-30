using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.GUI;
using RagadesCubeWin.GameLogic;
using RagadesCubeWin.GameLogic.InputSchemes;
using RagadesCubeWin.GameLogic.Rules;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.States.MainMenu;



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
            BitmapFont mediumFont = _fontManager.GetFont("Ragade's Cube Medium");
            BitmapFont smallFont = _fontManager.GetFont("Ragade's Cube Small");

            _titleText.Text = "New Game";

            // Players
            RCText playerText = new RCText(mediumFont, 1, 1, 600, 75);
            playerText.Text = "How many players?";
            playerText.CenterText = true;
            _menuPane.AddChild(playerText, 0, 175, 0.0f);

            // Player buttons

            
            RCButton playerButton1 = new RCButton(1, 1, 215, 60, smallFont);
            playerButton1.buttonText.Text = "1 Player";
            //newGame.AfterPressedAndReleased += PushGamePlayState;
            _menuPane.AddChild(playerButton1, 50, 225, 0.0f);


            RCButton playerButton2 = new RCButton(1, 1, 215, 60, smallFont);
            playerButton2.buttonText.Text = "2 Players";
            //newGame.AfterPressedAndReleased += PushGamePlayState;
            _menuPane.AddChild(playerButton2, 150, 300, 0.0f);


            RCButton playerButton3 = new RCButton(1, 1, 215, 60, smallFont);
            playerButton3.buttonText.Text = "3 Players";
            //newGame.AfterPressedAndReleased += PushGamePlayState;
            _menuPane.AddChild(playerButton3, 250, 375, 0.0f);

            RCButton playerButton4 = new RCButton(1, 1, 215, 60, smallFont);
            playerButton4.buttonText.Text = "4 Players";
            //newGame.AfterPressedAndReleased += PushGamePlayState;
            _menuPane.AddChild(playerButton4, 350, 450, 0.0f);
                
        }

        void PushGamePlayState()
        {
            RCGamePlayState gameState = new RCGamePlayState(Game);
            RCGameLogic gameLogic = new RCGameLogic(Game, gameState);

            //RCGLKeyboardInputScheme k1 = new RCGLKeyboardInputScheme();
            //gameLogic.AddPlayer(k1);

            RCGLGamePadInputScheme gp = new RCGLGamePadInputScheme(PlayerIndex.One);
            gameLogic.AddPlayer(gp);

            //RCGLKeyboardInputScheme k3 = new RCGLKeyboardInputScheme();
            //k3.LeftPressKey = Keys.Right;
            //k3.RightPressKey = Keys.Left;
            //k3.UpPressKey = Keys.Up;
            //k3.DownPressKey = Keys.Down;
            //gameLogic.AddPlayer(k3);

            //RCGLKeyboardInputScheme k2 = new RCGLKeyboardInputScheme();
            //gameLogic.AddPlayer(k2);

            gameLogic.Start(new RCDefaultGameRules(gameLogic));
            gameManager.PushState(gameState);
        }
    }
}
