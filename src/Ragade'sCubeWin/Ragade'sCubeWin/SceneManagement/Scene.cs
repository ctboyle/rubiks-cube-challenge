using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.SceneManagement
{
    public class RCScene
    {
        private string _cameraLabel;
        private RCSpatial _sceneRoot;
        private bool _isLoaded;

        public string SceneCameraLabel
        {
            get { return _cameraLabel; }
            set { _cameraLabel = value; }
        }

        public RCSpatial SceneRoot
        {
            get { return _sceneRoot; }
            set { _sceneRoot = value; }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        public RCScene(
            RCSpatial sceneRoot,
            string cameraLabel
            )
        {
            _isLoaded = false;
            _sceneRoot = sceneRoot;
            _cameraLabel = cameraLabel;

        }

        public void Load(
            GraphicsDevice graphicsDevice,
            ContentManager contentManager
            )
        {
            if (_sceneRoot != null)
            {
                _sceneRoot.LoadGraphicsContent(
                    graphicsDevice,
                    contentManager
                    );

                _isLoaded = true;
            }
        }

        public void Unload()
        {
            if (_sceneRoot != null)
            {
                _sceneRoot.UnloadGraphicsContent();
            }
        }


        public void Draw(
            GraphicsDevice graphicsDevice
            )
        {
            if (_cameraLabel != null && _sceneRoot != null)
            {
                RCCameraManager.SetActiveCamera(_cameraLabel);
                RCRenderManager.DrawScene(_sceneRoot);
            }
        }

        public void Update(
            GameTime gameTime
            )
        {
            if (_sceneRoot != null)
            {
                _sceneRoot.UpdateGS(gameTime, true);
            }
        }


    }
}
