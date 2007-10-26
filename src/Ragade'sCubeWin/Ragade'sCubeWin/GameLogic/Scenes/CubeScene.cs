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
        public RCCubeScene(RCCube theCube, string cameraLabel)
            : base(null, cameraLabel)
        {

            Camera.LocalTrans = Matrix.Invert(
                Matrix.CreateLookAt(
                    new Vector3(10.0f, 10.0f, 10.0f),
                    Vector3.Zero,
                    Vector3.Up
                )
            );

            RCDirectionalLight lightNode = new RCDirectionalLight(
                RCRenderManager.DirectionalLightIndex.Light0
            );

            lightNode.AddChild(theCube);
            lightNode.AddChild(Camera);

            lightNode.LightSource.LocalTrans = Matrix.Invert(
               Matrix.CreateLookAt(
                   new Vector3(1.0f, 1.0f, 10.0f),
                   Vector3.Zero,
                   Vector3.Up
               )
            );

            Camera.AddChild(lightNode.LightSource);

            lightNode.Diffuse = new Vector3(1.2f);
            lightNode.Specular = new Vector3(8.0f);

            _sceneRoot = lightNode;
        }
    }
}
