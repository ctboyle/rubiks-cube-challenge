using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.GraphicsManagement;
using System.Diagnostics;
using RagadesCubeWin.Cameras;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.States.MainMenu
{
    class RCMainMenuScene : RCScene
    {
        private RCCube menuCube;
        MainMenuCubeController cubeController;
        RCMenuCameraController cameraController;


        public RCMainMenuScene(Viewport screenViewport)
            :base(null,"")
        {
            CreateCamera(screenViewport);
            CreateCube();

            RCDirectionalLight sceneRoot = new RCDirectionalLight(
                RCRenderManager.DirectionalLightIndex.Light0
                );
            sceneRoot.AddChild(menuCube);
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
            menuCube = new RCCube(3, 3, 3);
            cubeController = new MainMenuCubeController();

            if (!cubeController.AttachToObject(menuCube))
            {
                Debug.Assert(false);
            }
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


            cameraController = new RCMenuCameraController();
            if (!cameraController.AttachToObject(camera))
            {
                Debug.Assert(false);
            }

        }
        

        public void MoveCamera(
            RCMenuCameraController.CameraPositions newPos
            )
        {
            cameraController.GoToPosition(
                newPos
                );
        }
    }
}
