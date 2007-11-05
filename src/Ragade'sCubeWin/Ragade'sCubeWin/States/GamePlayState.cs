using System;
using RagadesCubeWin.StateManagement;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.GameLogic;
using System.Collections.Generic;
using RagadesCubeWin.GraphicsManagement;
using Microsoft.Xna.Framework;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.Animation.Controllers;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GameLogic.InputSchemes;
using RagadesCubeWin.Input;
using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.GUI;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.States.Controllers;
using RagadesCubeWin.GameLogic.Rules;
using RagadesCubeWin.SoundManagement;

namespace RagadesCubeWin.States
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
                //gameManager.PopState();

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

            _screen = new RCScreenScene(graphics.GraphicsDevice.Viewport);
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
            _screen.Camera.ClearScreen = false;
            _sceneManager.AddScene(_screen);





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

        protected internal override void StateChanged(RCGameState newState, RCGameState oldState)
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