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

namespace RagadesCubeWin.States
{
    public class RCGamePlayState : RCGameState
    {
        private Dictionary<int, RCScene> _sceneByPlayer;

        public RCGamePlayState(Game game)
            : base(game)
        {
            _sceneByPlayer = new Dictionary<int, RCScene>();
        }

        public override void Initialize()
        {
            _sceneManager.AddScene(
                new FPScreenScene(graphics.GraphicsDevice.Viewport, Game.Services)
            );

            base.Initialize();
        }

        public void ApplyInputScheme(RCGameLogic gameLogic, RCInputScheme<RCGameLogic> inputScheme)
        {
            inputScheme.Apply(input, gameLogic);
        }

        public void AnnounceWinner(string winMessage)
        {
        }

        public void AddCubeScene(int playerIndex, IRCSceneCreator sceneCreator)
        {
            RCScene scene = sceneCreator.CreateScene();
            _sceneManager.AddScene(scene);
            _sceneByPlayer.Add(playerIndex, scene);
            UpdateSceneViewpoints();
        }

        public void RemoveCubeScene(int playerIndex)
        {
            _sceneManager.RemoveScene(_sceneByPlayer[playerIndex]);
            _sceneByPlayer.Remove(playerIndex);
            UpdateSceneViewpoints();
        }

        public void CloseState()
        {
            gameManager.PopState();
        }

        private void UpdateSceneViewpoints()
        {
            Viewport vp = graphics.GraphicsDevice.Viewport;
            int topRowPlayers = (int)Math.Ceiling(_sceneByPlayer.Count / 2.0);

            if (topRowPlayers == 0)
                return;

            vp.X = 0;
            vp.Y = 0;
            vp.Width /= topRowPlayers;
            vp.Height /= (_sceneByPlayer.Count > 1) ? 2 : 1;

            foreach (int pi in _sceneByPlayer.Keys)
            {
                RCScene checkScene = _sceneByPlayer[pi];
                RCCamera camera = RCCameraManager.GetCamera(checkScene.SceneCameraLabel);

                if (pi > topRowPlayers - 1) // bottom row
                {
                    vp.Y = vp.Height;
                    vp.X = (pi - topRowPlayers) * vp.Width;
                }
                else // top row
                {
                    vp.X = pi * vp.Width;
                }

                camera.Viewport = vp;
            }
        }
    }
}