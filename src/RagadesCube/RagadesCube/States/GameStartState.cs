using System;
using RC.Engine.StateManagement;
using Microsoft.Xna.Framework;
using RagadesCube.GameLogic;
using RC.Gui;
using RagadesCube.GameLogic.Rules;
using RC.Gui.Primitives;
using RC.Gui.Fonts;
using Microsoft.Xna.Framework.Graphics;
using RagadesCube.GameLogic.InputSchemes;
using RC.Engine.SceneManagement;
using System.Collections.Generic;
using RC.Engine.Input;
using RC.Engine.Cameras;
using RC.Engine.SoundManagement;

namespace RagadesCube.States
{
    class RCGameStartState : RCGameState
    {
        private RCGameLogic _logic;
        private RCScene[] _scenes;
        private RCGLInputScheme[] _inputSchemes;
        protected RCScreenScene _screen;
        protected RCText _titleText;

        public RCGameStartState(Game game, IRCGameRules rules, RCGLInputScheme[] inputSchemes)
            : base(game)
        {
            _logic = new RCGameLogic(Game);
            _logic.Rules = rules;
            _scenes = new RCScene[inputSchemes.Length];
            _inputSchemes = new RCGLInputScheme[inputSchemes.Length];

            Array.Copy(inputSchemes, _inputSchemes, inputSchemes.Length);

            for (int i = 0; i < inputSchemes.Length; ++i)
            {
                RCScene scene = null;
                IRCGamePlayerViewer gpView = _logic.AddPlayer(out scene);
                _sceneManager.AddScene(scene);
                _scenes[i] = scene;
            }

            UpdateSceneViewpoints();
        }

        public override void Initialize()
        {
            // kill music beat and everything else here
            //SoundManager.Stop();

            _logic.Shuffle();

            IFontManager fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));
            BitmapFont mediumFont = fontManager.GetFont("Ragade's Cube Medium");

            _screen = new RCScreenScene(graphics.GraphicsDevice.Viewport);
            _titleText = new RCText(
               mediumFont,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height / 2,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height / 2
            );
            _titleText.Text = "Shuffling...";
            _titleText.Color = Color.Maroon;
            _titleText.CenterText = true;
            _screen.ScreenPane.AddChild(_titleText, 0, 0, 1f);
            _sceneManager.AddScene(_screen);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!_logic.IsShuffling)   
            {
                gameManager.PopState();
                gameManager.PushState(
                    new RCGamePlayState(
                        Game, 
                        _logic, 
                        _scenes, 
                        _inputSchemes
                    )
                );
                _logic.StartGame();
            }
  
            base.Update(gameTime);
        }

        private void UpdateSceneViewpoints()
        {
            Viewport vp = graphics.GraphicsDevice.Viewport;
            int topRowPlayers = (int)Math.Ceiling(_scenes.Length / 2.0);

            if (topRowPlayers == 0)
                return;

            vp.X = 0;
            vp.Y = 0;
            vp.Width /= topRowPlayers;
            vp.Height /= (_scenes.Length > 1) ? 2 : 1;

            for (int i = 0; i < _scenes.Length; ++i)
            {
                RCScene checkScene = _scenes[i];
                RCCamera camera = RCCameraManager.GetCamera(checkScene.SceneCameraLabel);

                if (i > topRowPlayers - 1) // bottom row
                {
                    vp.Y = vp.Height;
                    vp.X = (i - topRowPlayers) * vp.Width;
                }
                else // top row
                {
                    vp.X = i * vp.Width;
                }

                camera.Viewport = vp;
            }
        }
    }
}