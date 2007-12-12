using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using RC.Engine.Rendering;
using RC.Gui.Primitives;
using RagadesCube.Controllers;
using RagadesCube.SceneObjects;
using RC.Engine.SceneManagement;
using RC.Engine.GraphicsManagement;
using RC.Engine.Cameras;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace RagadesCube.Scenes
{
    class RCMenuCubeScene : RCScene
    {
        private RCCube _menuCube;
        private RCMenuCameraController _cameraController;

        public RCQuad _background;

        public Texture2D Background
        {
            get { return _background.Image; }
            set { _background.Image = value; } 
        }

        public RCCube Cube 
        {
            get { return _menuCube; }
        }

        public RCMenuCameraController CameraController
        {
            get { return _cameraController; }
        }

        public RCMenuCubeScene(Viewport screenViewport)
            :base(null,"")
        {
            CreateCamera(screenViewport);
            CreateCube();

            RCDirectionalLight sceneRoot = new RCDirectionalLight(
                RCRenderManager.DirectionalLightIndex.Light0
                );
            sceneRoot.AddChild(_menuCube);
            sceneRoot.AddChild(Camera);

            sceneRoot.LightSource.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(1.0f, 1.0f, 10.0f),
                Vector3.Zero,
                Vector3.Up
            ));

            Camera.AddChild(sceneRoot.LightSource);
            sceneRoot.Diffuse = new Vector3(0.7f);
            sceneRoot.Specular = new Vector3(0.5f) ;

            _sceneRoot = sceneRoot;

        }

        private void CreateCube()
        {
            _menuCube = new RCCube(3, 3, 3);
        }

        private void CreateCamera(Viewport cameraViewport)
        {
            _cameraLabel = "MainMenuCamera";
            RCPerspectiveCamera camera = new RCPerspectiveCamera(cameraViewport);
            RCCameraManager.AddCamera(camera, _cameraLabel);
            camera.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(0.0f, 0.0f, 25.0f),
                Vector3.Zero,
                Vector3.Up
                ));


            _cameraController = new RCMenuCameraController();
            if (!_cameraController.AttachToObject(camera))
            {
                Debug.Assert(false);
            }

            camera.UpdateGS(null, true);

            CreateBackground();

        }

        private void CreateBackground()
        {
            Vector3 upperLeft = Camera.Viewport.Unproject(
                new Vector3(0.0f, 0.0f, 1.0f),
                Camera.Projection,
                Camera.View,
                Matrix.Identity
                );

            Vector3 lowerRight = Camera.Viewport.Unproject(
                new Vector3(Camera.Viewport.Width, Camera.Viewport.Height, 1.0f),
                Camera.Projection,
                Camera.View,
                Matrix.Identity
                );

            Vector3 diff = upperLeft - lowerRight;

            _background = new RCQuad(-diff.X, diff.Y, 1, 1);
            Camera.AddChild(_background);
            
            _background.LocalTrans =
                Matrix.CreateTranslation(
                    new Vector3(upperLeft.X, upperLeft.Y, -Camera.Far)
                    );
        }

        public override void Load(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            base.Load(graphicsDevice, contentManager);

            Background = contentManager.Load<Texture2D>("Content\\Textures\\Background1");
        }

        public void MoveCamera(
            RCMenuCameraController.CameraPositions newPos
            )
        {
            _cameraController.GoToPosition(
                newPos
                );
        }
    }
}
