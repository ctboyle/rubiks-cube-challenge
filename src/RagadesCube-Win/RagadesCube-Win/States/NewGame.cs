using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RC.Gui;
using RagadesCube.GameLogic;
using RagadesCube.GameLogic.InputSchemes;
using RagadesCube.GameLogic.Rules;
using RC.Gui.Fonts;
using RC.Gui.Primitives;
using RagadesCube.States;
using RC.Gui.Controls.Control_Subclasses;
using RagadesCube.Misc;
using RagadesCube.Controllers;



namespace RagadesCube.States
{
    class RCNewGame : RCCubeMenu
    {
        public enum SpinnerIndex
        {
            Player1 = 0,
            Player2,
            Player3,
            Player4
        }

        private RCSpinner[] _playerSpinners;
        private PlayerInputSpinnerManager _spinnerManager;


        public RCNewGame(Game game)
            : base(game)
        {
            // Set this menu position on the cube.
            _menuPos = RCMenuCameraController.CameraPositions.Right;

            _playerSpinners = new RCSpinner[4];
            _spinnerManager = new PlayerInputSpinnerManager();
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

            CreateSpinners(100 ,180 ,275, 25, smallFont);

            // Players
            RCText playerText = new RCText(mediumFont, 1, 1, 600, 75);
            playerText.Text = "Choose who will play.";
            playerText.CenterText = true;
            _menuPane.AddChild(playerText, 0, 175, 0.0f);

            RCButton startGameButton = new RCButton(1, 1, 200, 50, smallFont);
            startGameButton.buttonText.Text = "  Start Game  ";
            startGameButton.buttonText.SizeTextToWidth = true;

            startGameButton.AfterPressedAndReleased += StartGame;

            _menuPane.AddChild(startGameButton, 200, 450, 0.0f);

                           
        }

        private void CreateSpinners(
            int width,
            int height,
            int yVerticalPos,
            int xHorizontalSpacing,
            BitmapFont spinnerFont
            )
        {
            // Calculate the total width of the 4 sinners so that
            // we can center them on the menucube face.
            int spinnerGroupWidth = 4 * width + 3 * xHorizontalSpacing;
            int xSpinnerStart = _menuPane.ScreenWidth/2 - spinnerGroupWidth / 2;

            Vector2 spinnerPos = new Vector2(xSpinnerStart, yVerticalPos);

            Array playerIndicies = Enum.GetValues(typeof(PlayerIndex));

            for (int playerIndex = 0; playerIndex < playerIndicies.Length; playerIndex++ )
            {
                // Create and position the spinner at the current spinner pos.
                _playerSpinners[playerIndex] = new RCSpinner(
                    width, height,
                    width, height,
                    spinnerFont
                    );

                _menuPane.AddChild(
                    _playerSpinners[playerIndex],
                    (int)spinnerPos.X, (int)spinnerPos.Y,
                    0.0f
                    );

                // Add label over spinner

                RCText label = new RCText(spinnerFont, 1, 1, width, 75);
                label.Text = "Player " + (playerIndex + 1).ToString();
                label.SizeTextToWidth = true;
                _menuPane.AddChild(label, (int)spinnerPos.X, (int)spinnerPos.Y - 25, 0.0f);


                spinnerPos.X += width + xHorizontalSpacing;

                


            }

            // Tell the spinner manager to initialize the newly created spinners.
            _spinnerManager.InitializeSpinners(_playerSpinners, spinnerFont);
        }

        private void StartGame()
        {
            RCGLInputScheme[] inputSchemes = _spinnerManager.GetPlayerSchemes();

            if (inputSchemes.Length != 0)
            {
                IRCGameRules rules = new RCDefaultGameRules();

                RCGameStartState gss = new RCGameStartState(Game, rules, inputSchemes);

                gameManager.PushState(new FadeState(Game, gss));
            }
        }
    }
}
