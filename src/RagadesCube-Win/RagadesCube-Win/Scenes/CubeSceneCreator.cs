using System;
using RC.Engine.SceneManagement;
using RagadesCube.SceneObjects;
using Microsoft.Xna.Framework.Graphics;

namespace RagadesCube.Scenes
{
    public class RCCubeSceneCreator : IRCSceneCreator
    {
        private RCCube _cube;
        private Viewport _sceneViewport;

        public void AttachCube(RCCube cube)
        {
            _cube = cube;
        }

        public Viewport SceneViewport
        {
            get { return _sceneViewport; }
            set { _sceneViewport = value; }
        }

        public RCScene CreateScene()
        {
            if (_cube == null)
                throw new Exception("Unable to create scene because no cube attached.");

            return new RCCubeScene(_sceneViewport, _cube);
        }
    }
}
