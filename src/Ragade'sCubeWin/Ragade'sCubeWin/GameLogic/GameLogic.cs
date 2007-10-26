using System;
using System.Collections.Generic;
using RagadesCubeWin.States;
using Microsoft.Xna.Framework;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Animation.Controllers;
using RagadesCubeWin.Input;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.GameLogic.InputSchemes;
using RagadesCubeWin.GameLogic.Scenes;
using RagadesCubeWin.GameLogic.Rules;

namespace RagadesCubeWin.GameLogic
{
    public class RCGameLogic : GameComponent, IDisposable
    {
        private List<RCGamePlayer> _players;
        private RCGamePlayState _gamePlayState;
        private IRCGameRules _rules;

        public static int MaxPlayers
        {
            get { return Enum.GetValues(typeof(RCPlayerIndex)).Length; }
        }

        public RCGameLogic(Game game, RCGamePlayState gps) 
            : base(game)
        {
            Game.Components.Add(this);
            _players = new List<RCGamePlayer>();
            _gamePlayState = gps;
        }

        public bool IsPlaying
        {
            get { return (Enabled && _rules != null); }
        }

        public int PlayerCount
        {
            get { return _players.Count; }
        }

        public override void Update(GameTime gameTime)
        {
            _rules.Update(gameTime);
            
            RCPlayerIndex winnerIndex;
            if (_rules.GetWinner(out winnerIndex))
                _gamePlayState.AnnounceWinner(winnerIndex.ToString());

            base.Update(gameTime);
        }

        public void AddPlayer(RCGLInputScheme inputScheme)
        {
            if(IsPlaying)
                throw new Exception("Cannot add player during game play.");

            if (_players.Count == MaxPlayers)
                throw new Exception("The maximum amount of players already have joined the game.");

            RCActionCube cube = new RCActionCube(Game);
            RCGamePlayer player = new RCGamePlayer(cube, (RCPlayerIndex)(_players.Count));
            _players.Add(player);
            
            RCCubeSceneCreator sceneCreator = new RCCubeSceneCreator();
            player.AttachToScene(sceneCreator);
            inputScheme.AttachPlayer(player);
            _gamePlayState.ApplyInputScheme(this, inputScheme);
            _gamePlayState.AddCubeScene((int)player.Index, sceneCreator);
        }

        public void RemovePlayer()
        {
            if (_players.Count == 0)
                return;

            int index = _players.Count - 1;
            _players.RemoveAt(index);
            _gamePlayState.RemoveCubeScene(index);
        }

        public void MovePlayerCube(RCPlayerIndex playerIndex, Vector3 axis, Vector2 where)
        {
            if (IsPlaying && _rules.PlayerMoveCube(playerIndex))
            {
                RCGamePlayer player = _players[(int)playerIndex];
                player.MyCube.Move(axis, where);
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

        public void Start(IRCGameRules rules)
        {
            if (rules == null)
                throw new NullReferenceException("Rules must be specified for the game to start.");

            _rules = rules;
            _rules.Reset();
        }

        public void Stop()
        {
            _rules = null;

            while (_players.Count != 0)
                RemovePlayer();
           
            _gamePlayState.CloseState();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Stop();

            Game.Components.Remove(this);

            base.Dispose(disposing);
        }
    }
}
