using System;
using RC.Engine.StateManagement;
using RC.Engine.SceneManagement;
using RagadesCube.GameLogic;
using System.Collections.Generic;
using RC.Engine.GraphicsManagement;
using Microsoft.Xna.Framework;
using RagadesCube.SceneObjects;
using RC.Engine.Cameras;
using RagadesCube.Controllers;
using Microsoft.Xna.Framework.Graphics;
using RagadesCube.GameLogic.InputSchemes;
using RC.Engine.Input;
using RC.Gui.Panes;
using RC.Gui;
using RC.Gui.Primitives;
using RC.Gui.Fonts;
using RagadesCube.Controllers;
using RagadesCube.GameLogic.Rules;
using RC.Engine.SoundManagement;

namespace RagadesCube.States
{
    public class RCGamePlayState : RCGameState
    {
        private TimeSpan _last;
        private RCGameLogic _logic;
        private RCScene[] _cubeScenes;
        private RCGLInputScheme[] _inputSchemes;
        protected RCScreenScene[] _infoScenes;
        protected RCScreenScene _screen;
        protected RCText _titleText;
        protected RCText[] _playerText;
        protected RCButton _exitButton;
        protected RCGUIManager _guiManager;
        protected GuiInputScheme _guiInputScheme;
      
        public RCGamePlayState(
            Game game, 
            RCGameLogic logic, 
            RCScene[] scenes, 
            RCGLInputScheme[] inputSchemes
        )
            : base(game)
        {
            _logic = logic;
            _cubeScenes = scenes;
            _inputSchemes = inputSchemes;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_guiInputScheme != null)
                    _guiInputScheme.Unapply();

                for (int i = 0; i < _inputSchemes.Length; ++i)
                    _inputSchemes[i].Unapply();
            }

            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            if (_logic.IsPlaying)
            {
                _last = _last.Add(gameTime.ElapsedRealTime);
                
                _titleText.Text = string.Format(
                    "{0} hr {1} min {2} sec", 
                    _last.Hours, 
                    _last.Minutes, 
                    _last.Seconds
                );

                foreach (IRCGamePlayerViewer player in _logic.GetPlayers())
                {
                    _playerText[(int)player.Index].Text = string.Format(
                        "move count {0}",
                        player.CubeView.MoveCount
                    );
                }
            }
            else
            {
                gameManager.PopState();

                List<IRCGamePlayerViewer> winners = new List<IRCGamePlayerViewer>();
                IRCGamePlayerViewer[] players = _logic.GetPlayers();

                foreach (IRCGamePlayerViewer player in players)
                {
                    if (player.CubeView.IsSolved)
                        winners.Add(player);
                }

                if (winners.Count > 0)
                {
                    RCGameCompleteState gcs = new RCGameCompleteState(Game, winners.ToArray());
                    gameManager.PushState(gcs);
                }
            }

            base.Update(gameTime);
        }

        public override void Initialize()
        {
            // kill music beat and everything else here
            //SoundManager.Stop();


            for (int i = 0; i < _inputSchemes.Length; ++i)
            {
                IRCGamePlayerViewer player = _logic.GetPlayer((RCPlayerIndex)i);
                _inputSchemes[i].AttachPlayer(player);
                _inputSchemes[i].Apply(input, _logic);
            }

            for (int i = 0; i < _cubeScenes.Length; ++i)
                _sceneManager.AddScene(_cubeScenes[i]);

            IFontManager fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));

            BitmapFont largeFont = fontManager.GetFont("Ragade's Cube Large");
            BitmapFont mediumFont = fontManager.GetFont("Ragade's Cube Small");
            BitmapFont smallFont = fontManager.GetFont("Lucida Console");

            _screen = new RCScreenScene(graphics.GraphicsDevice.Viewport);
            _screen.Camera.ClearScreen = false;
            _sceneManager.AddScene(_screen);

            _titleText = new RCText(
               largeFont,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height / 2,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height / 2
            );
            _titleText.Text = "Hello World";
            _titleText.CenterText = true;
            _titleText.Color = Color.Maroon;
            _screen.ScreenPane.AddChild(_titleText, 0, 0, 1f);

            if (Array.Find<RCGLInputScheme>(
                _inputSchemes, 
                delegate(RCGLInputScheme scheme) 
                { 
                    return scheme is RCGLMouseInputScheme; 
                }
            ) != null)
            {
                _exitButton = new RCButton(
                   1,
                   1,
                   70,
                   25,
                   smallFont
                );
                _exitButton.buttonText.Text = "Exit";
                _exitButton.buttonText.CenterText = true;
                _exitButton.buttonText.Color = Color.DarkSlateBlue;
                _exitButton.AfterPressedAndReleased += delegate()
                {
                    gameManager.PopState();
                };

                _screen.ScreenPane.AddChild(
                    _exitButton, 
                    graphics.GraphicsDevice.Viewport.Width - 70, 
                    0, 
                    1.0f
                );

                _guiManager = new RCGUIManager(_screen);
                _guiInputScheme = new GuiInputScheme();
                _guiInputScheme.Apply(input, _guiManager);
            }

            _playerText = new RCText[_cubeScenes.Length];
            _infoScenes = new RCScreenScene[_cubeScenes.Length];

            for (int i = 0; i < _cubeScenes.Length; ++i)
            {
                Viewport vp = _cubeScenes[i].Camera.Viewport;
                _infoScenes[i] = new RCScreenScene(vp);
                _playerText[i] = new RCText(
                   mediumFont,
                   vp.Width,
                   vp.Height / 2,
                   vp.Width,
                   vp.Height / 2
                );
                _playerText[i].Color = Color.Red;
                _playerText[i].CenterText = true;
                _infoScenes[i].ScreenPane.AddChild(_playerText[i], 0, vp.Height-30, 1f);
                _infoScenes[i].Camera.ClearScreen = false;
                _sceneManager.AddScene(_infoScenes[i]);
            }

            base.Initialize();
        }

        protected override void StateChanged(RCGameState newState, RCGameState oldState)
        {
            if ((oldState == this) && (newState is RCGameCompleteState))
            {
                this.Visible = true;
                this.Enabled = false;
            }
            else
            {
                base.StateChanged(newState, oldState);
            }
        }
    }
}