using System;
using RC.Engine.StateManagement;
using Microsoft.Xna.Framework;
using RagadesCube.GameLogic;
using RC.Gui.Primitives;
using RC.Gui.Fonts;
using RC.Gui;
using Microsoft.Xna.Framework.Graphics;
using RagadesCube.Controllers;
using RC.Input.Watchers;
using RC.Input.Events;
using RC.Input.Types;
using Microsoft.Xna.Framework.Input;
using RagadesCube.States;
using RC.Engine.SoundManagement;

namespace RagadesCube.States
{
    class RCGameCompleteState : RCGameState
    {
        bool inputReady;

        private RCText _outText;
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

            SoundManager.PlayCue("finish");

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

#if !XBOX

            MouseWatcher mouseWatcher = new MouseWatcher();
            mouseWatcher.WatchEvent(new MouseEvent(
                MouseInput.LeftButton,
                EventTypes.OnUp,
                delegate(Vector2 pos, Vector2 move)
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));



            mouseWatcher.WatchEvent(new MouseEvent(
                MouseInput.RightButton,
                EventTypes.OnUp,
                delegate(Vector2 pos, Vector2 move)
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));

#endif

            XBox360GamePad gamepadWatcher1 = new XBox360GamePad(PlayerIndex.One);
            gamepadWatcher1.WatchEvent(new XBox360GamePadEvent(
                XBox360GamePadTypes.A,
                EventTypes.OnDown,
                delegate()
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));

            XBox360GamePad gamepadWatcher2 = new XBox360GamePad(PlayerIndex.Two);
            gamepadWatcher2.WatchEvent(new XBox360GamePadEvent(
                XBox360GamePadTypes.A,
                EventTypes.OnDown,
                delegate()
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));
            XBox360GamePad gamepadWatcher3 = new XBox360GamePad(PlayerIndex.Three);
            gamepadWatcher3.WatchEvent(new XBox360GamePadEvent(
                XBox360GamePadTypes.A,
                EventTypes.OnDown,
                delegate()
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));
            XBox360GamePad gamepadWatcher4 = new XBox360GamePad(PlayerIndex.Four);
            gamepadWatcher4.WatchEvent(new XBox360GamePadEvent(
                XBox360GamePadTypes.A,
                EventTypes.OnDown,
                delegate()
                {
                    if (inputReady)
                    {
                        GoToMainMenu();
                    }
                }));

            input.AddWatcher(keyWatcher);
            input.AddWatcher(gamepadWatcher1);
            input.AddWatcher(gamepadWatcher2);
            input.AddWatcher(gamepadWatcher3);
            input.AddWatcher(gamepadWatcher4);

#if !XBOX

            input.AddWatcher(mouseWatcher);

#endif
            
            base.Initialize();
        }

        private void GoToMainMenu()
        {

            FadeState fadeState = new FadeState(Game, new RCTitleScreenState(Game));
            gameManager.ChangeState(fadeState);
        }
    }
}
