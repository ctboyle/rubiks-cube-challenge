using System;
using System.Collections.Generic;
using RagadesCube.States;
using Microsoft.Xna.Framework;
using RagadesCube.SceneObjects;
using RagadesCube.Controllers;
using RC.Engine.Input;
using RC.Engine.SceneManagement;
using RC.Engine.Cameras;
using RagadesCube.GameLogic.InputSchemes;
using RagadesCube.Scenes;
using RagadesCube.GameLogic.Rules;
using Microsoft.Xna.Framework.Graphics;

namespace RagadesCube.GameLogic
{
    public class RCGameLogic : GameComponent, IDisposable
    {
        private bool _isPlaying;
        private int _shuffleCount;
        private List<RCGamePlayer> _players;
        private IRCGameRules _rules;

        public static int MaxPlayers
        {
            get { return Enum.GetValues(typeof(RCPlayerIndex)).Length; }
        }

        public RCGameLogic(Game game) 
            : base(game)
        {
            _isPlaying = false;
            Game.Components.Add(this);
            _players = new List<RCGamePlayer>();
        }

        public IRCGameRules Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }

        public bool IsShuffling
        {
            get { return _shuffleCount != 0; }
        }

        public bool IsPlaying
        {
            get { return (_isPlaying && !IsShuffling); }
        }

        public int PlayerCount
        {
            get { return _players.Count; }
        }

        public override void Update(GameTime gameTime)
        {
            // BUG BUG: _rules.CubeShuffler.IsShuffling always retruns false.
            // _cubes are set upon Shuffle which is called later.
            if (_shuffleCount != 0 && !_rules.CubeShuffler.IsShuffling)
            {
                List<RCActionCube> cubes = new List<RCActionCube>();
                foreach (RCGamePlayer player in _players)
                    cubes.Add(player.MyCube);
                _rules.CubeShuffler.Shuffle(cubes.ToArray());
                _shuffleCount--;
            }

            _rules.Update(gameTime);

            base.Update(gameTime);
        }

        public IRCGamePlayerViewer AddPlayer(out RCScene createdScene)
        {
            if(IsPlaying)
                throw new Exception("Cannot add player during game play.");

            if (_players.Count == MaxPlayers)
                throw new Exception("The maximum amount of players already have joined the game.");

            RCActionCube cube = new RCActionCube(Game);
            RCCubeScene scene = cube.CreateScene(new Viewport());
            RCGamePlayer player = new RCGamePlayer(cube, (RCPlayerIndex)(_players.Count), scene.Camera);
            _players.Add(player);

            createdScene = scene;

            cube.RotateAnimationComplete += OnPlayerMoveComplete;

            return player;
        }

        public void MovePlayerCube(RCPlayerIndex playerIndex, Vector3 xAxis, Vector3 yAxis, Vector2 where)
        {
            if (IsPlaying && _rules.PlayerMoveCube(playerIndex))
            {
                RCGamePlayer player = _players[(int)playerIndex];
                player.MyCube.Move(xAxis, yAxis, where);
            }
        }

        public void SelectPlayerCube(RCPlayerIndex playerIndex, RCCube.FaceSide selectedFace)
        {
            if (IsPlaying && _rules.PlayerSelectCube(playerIndex))
            {
                RCGamePlayer player = _players[(int)playerIndex];
                player.MyCube.Select(selectedFace);
            }
        }

        public void RotatePlayerCube(RCPlayerIndex playerIndex, RCCube.RotationDirection rotDir)
        {
            if (IsPlaying && _rules.PlayerRotateCube(playerIndex))
            {
                RCGamePlayer player = _players[(int)playerIndex];
                if(!player.MyCube.IsRotating)
                    player.MyCube.Rotate(rotDir);
            }
        }

        public IRCGamePlayerViewer GetPlayer(RCPlayerIndex playerIndex)
        {
            IRCGamePlayerViewer player = _players[(int)playerIndex];
            return player;
        }

        public IRCGamePlayerViewer[] GetPlayers()
        {
            return (IRCGamePlayerViewer[])_players.ToArray();
        }

        public void Shuffle()
        {
            if (_rules == null)
                throw new NullReferenceException("Rules must be specified for the shuffle to start.");
            _shuffleCount = 5;
        }

        public void StartGame()
        {
            if (_rules == null)
                throw new NullReferenceException("Rules must be specified for the game to start.");

            _isPlaying = true;
            _rules.Reset(this);

            foreach (RCGamePlayer player in _players)
                player.MyCube.ResetMoveCount();
        }

        public void StopGame()
        {
            if (!IsPlaying)
                return;

            _rules.Stop();
            _isPlaying = false;
        }

        private void OnPlayerMoveComplete(RCActionCube playerCube)
        {
            if (IsPlaying && _rules.IsWinnerPresent)
                StopGame();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                StopGame();

            Game.Components.Remove(this);

            base.Dispose(disposing);
        }
    }
}
