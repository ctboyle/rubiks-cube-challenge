using System;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Cameras;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Animation.Controllers;

namespace RagadesCubeWin.GameLogic.Scenes
{
    public class RCCubeScene : RCScene
    {
        private static int CameraCount = 0;

        public RCCubeScene(RCCube theCube, string cameraLabel)
            : base(null, cameraLabel)
        {
            RCCamera mainCamera = RCCameraManager.GetCamera(cameraLabel);

            mainCamera.LocalTrans = Matrix.Invert(
                Matrix.CreateLookAt(
                    new Vector3(10, 10, 10),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 1, 0)
                )
            );

            RCSceneNode rootNode = new RCSceneNode();

            RCDirectionalLight lightNode = new RCDirectionalLight(
                RCRenderManager.DirectionalLightIndex.Light0
            );

            lightNode.Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            lightNode.Specular = new Vector3(1.0f, 1.0f, 1.0f);
            lightNode.Direction.Normalize();

            lightNode.AddChild(theCube);
            rootNode.AddChild(lightNode);
            rootNode.AddChild(mainCamera);

            _sceneRoot = rootNode;
        }
    }
}
