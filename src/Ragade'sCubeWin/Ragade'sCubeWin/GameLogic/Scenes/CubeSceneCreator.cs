using System;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.SceneObjects;

namespace RagadesCubeWin.GameLogic.Scenes
{
    public class RCCubeSceneCreator : IRCSceneCreator
    {
        private RCCube _cube;
        private string _cameraLabel;

        public void AttachCube(RCCube cube)
        {
            _cube = cube;
        }

        public void AttachCamera(string cameraLabel)
        {
            _cameraLabel = cameraLabel;
        }

        public RCScene CreateScene()
        {
            if (_cube == null)
                throw new Exception("Unable to create scene because no cube attached.");

            if (_cameraLabel == null)
                throw new Exception("Unable to create scene because no camera attached.");

            return new RCCubeScene(_cube, _cameraLabel);
        }
    }
}
