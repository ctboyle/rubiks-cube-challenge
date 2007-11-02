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
        RCCamera Camera { get; }
    }

    public class RCGamePlayer : IRCGamePlayerViewer
    {
        private RCPlayerIndex _index;
        private RCActionCube _myCube;
        private RCCamera _camera;

        public RCGamePlayer(RCActionCube cube, RCPlayerIndex index, RCCamera camera)
        {
            _index = index;
            _myCube = cube;
            _camera = camera;
        }

        public RCActionCube MyCube
        {
            get { return _myCube; }
        }

        public IRCCubeViewer CubeView
        {
            get { return _myCube; }
        }

        public RCCamera Camera
        {
            get { return _camera; }
        }

        public RCPlayerIndex Index
        {
            get { return _index; }
        }
    }
}
