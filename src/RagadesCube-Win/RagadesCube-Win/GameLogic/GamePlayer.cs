using System;
using RC.Engine.Cameras;
using RagadesCube.SceneObjects;
using Microsoft.Xna.Framework;
using RagadesCube.Controllers;
using RC.Engine.Input;
using RagadesCube.Scenes;

namespace RagadesCube.GameLogic
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
