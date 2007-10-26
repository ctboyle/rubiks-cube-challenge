using System;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.SceneObjects;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Animation.Controllers;
using RagadesCubeWin.Input;
using RagadesCubeWin.GameLogic.Scenes;

namespace RagadesCubeWin.GameLogic
{
    public interface IRCGamePlayerViewer
    {
        IRCCubeViewer CubeView { get; }
        RCPlayerIndex Index { get; }
        string CameraLabel { get; }
    }

    public class RCGamePlayer : IRCGamePlayerViewer, IDisposable
    {
        private static int CreatedPlayers = 0;

        private string _cameraName;
        private RCPlayerIndex _index;
        private RCActionCube _myCube;

        public RCGamePlayer(RCActionCube cube, RCPlayerIndex index)
        {
            _index = index;
            _myCube = cube;
            _cameraName = "RC Game Player Camera - " + (CreatedPlayers++).ToString();

            RCCameraManager.AddCamera(
                new RCPerspectiveCamera(new Microsoft.Xna.Framework.Graphics.Viewport()),
                _cameraName
            );
        }

        public void Dispose()
        {
            RCCameraManager.RemoveCamera(_cameraName);
        }

        public RCActionCube MyCube
        {
            get { return _myCube; }
        }

        public IRCCubeViewer CubeView
        {
            get { return _myCube; }
        }

        public string CameraLabel
        {
            get { return _cameraName; }
        }

        public RCPlayerIndex Index
        {
            get { return _index; }
        }

        public void AttachToScene(RCCubeSceneCreator sceneCreator)
        {
            _myCube.AttachToScene(sceneCreator);
            sceneCreator.AttachCamera(_cameraName);
        }
    }
}
