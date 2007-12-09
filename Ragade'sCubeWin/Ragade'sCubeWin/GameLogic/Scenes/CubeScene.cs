using System;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Cameras;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Animation.Controllers;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.GUI;

namespace RagadesCubeWin.GameLogic.Scenes
{
    public class RCCubeScene : RCScene, IDisposable
    {
        private static int CubeCount = 0;

        private RCQuad _background;

        public RCCubeScene(Viewport viewport, RCCube theCube)
            : base(null, "Cube Scene Camera " + (CubeCount++).ToString())
        {
            RCCameraManager.AddCamera(new RCPerspectiveCamera(viewport), _cameraLabel);

            Camera.ClearColor = Color.DodgerBlue;

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
            
            _background = new RCQuad(1, 1, 1, 1);
            Camera.AddChild(_background);
        }

        public void Dispose()
        {
            RCCameraManager.RemoveCamera(_cameraLabel);
        }

        public override void Load(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            _background.Image = contentManager.Load<Texture2D>("Content\\Textures\\yello002");
            base.Load(graphicsDevice, contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            Matrix inverseView = Matrix.Invert(Camera.View);

            Vector3 upperLeft = Camera.Viewport.Unproject(
                new Vector3(
                    Camera.Viewport.X + 1,
                    Camera.Viewport.Y + 1, 
                    Camera.Viewport.MaxDepth
                ),
                Camera.Projection,
                Camera.View,
                Camera.WorldTrans
            );

            Vector3 lowerRight = Camera.Viewport.Unproject(
                new Vector3(
                    Camera.Viewport.X + Camera.Viewport.Width - 1,
                    Camera.Viewport.Y + Camera.Viewport.Height - 1,
                    Camera.Viewport.MaxDepth
                ),
                Camera.Projection,
                Camera.View,
                Camera.WorldTrans
            );

            Vector3 diff = upperLeft - lowerRight;

            _background.WorldWidth = -diff.X;
            _background.WorldHeight = diff.Y;

            _background.LocalTrans = Matrix.CreateTranslation(
                new Vector3(upperLeft.X, upperLeft.Y, -Camera.Far)
            );

            base.Update(gameTime);
        }
    }
}
