using System;
using RagadesCubeWin.StateManagement;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GameLogic;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GUI;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.States.Controllers;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input.Types;
using Microsoft.Xna.Framework.Input;
using RagadesCubeWin.States.MenuCubeState.CubeMenus;
using RagadesCubeWin.States.TitleScreen;

namespace RagadesCubeWin.States
{
    class RCGameCompleteState : RCGameState
    {
        bool inputReady;

        private RCText _outText;
        private double _time;
        private IRCGamePlayerViewer[] _winners;

        public RCGameCompleteState(Game game, IRCGamePlayerViewer[] winners)
            : base(game)
        {
            inputReady = false;

            _winners = winners;

            // Create a viewport that is the size of the game window.
            Viewport gameScreen = new Viewport();

            gameScreen.X = 0;
            gameScreen.Y = 0;
            gameScreen.Width = game.Window.ClientBounds.Width;
            gameScreen.Height = game.Window.ClientBounds.Height;

                        

            IFontManager fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));
            BitmapFont mediumFont = fontManager.GetFont("Ragade's Cube Large");

            _outText = new RCText(
               mediumFont,
               gameScreen.Width,
               gameScreen.Height,
               gameScreen.Width,
               gameScreen.Height
            );

            _outText.Text = "Player " + (((int)winners[0].Index) + 1).ToString() + " Wins!";
            _outText.CenterText = true;
            _outText.Color = Color.Chocolate;
            
            

            RCWobbleController textWobble = new RCWobbleController();

            textWobble.Period = 2 * new Vector3(0.0f, 0.0f, 1.0f);
            textWobble.RotationAmplitude = new Vector3(
                MathHelper.ToRadians(0.0f),
                MathHelper.ToRadians(0.0f),
                MathHelper.ToRadians(5.0f)
                );

            ScaleController textScale = new ScaleController();
            textScale.AttachToObject(_outText);

            textScale.BeginAnimation(
                new Vector3(0.1f, 0.1f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                0.75f
                );

            textScale.OnComplete += delegate() { inputReady = true; };



            //int size = mediumFont.MeasureString(_outText.Text);


            textWobble.LocalPivot = new Vector3(
                gameScreen.Width/2,
                -25.0f,
                0.0f
                );

            textWobble.AttachToObject(_outText);


            RCScreenScene scene = new RCScreenScene(gameScreen);

            scene.Camera.ClearScreen = false;
            scene.Camera.ClearColor = Color.Black;
            scene.Camera.ClearOptions = ClearOptions.DepthBuffer;

            scene.ScreenPane.AddChild(
                _outText,
                0,
                gameScreen.Height / 2,
                1.0f
            );

            _sceneManager.AddScene(scene);
        }

        public override void Initialize()
        {
            inputReady = false;

            KeyboardWatcher keyWatcher = new KeyboardWatcher();
            keyWatcher.WatchEvent(new KeyboardEvent(
                EventTypes.OnDown,
                delegate(Keys key)
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));


            input.AddWatcher(keyWatcher);
            
            base.Initialize();
        }

        private void GoToMainMenu()
        {

            FadeState fadeState = new FadeState(Game, new RCTitleScreenState(Game));
            gameManager.ChangeState(fadeState);
        }
    }
}
