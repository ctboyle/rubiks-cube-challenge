#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;



using RagadesCubeWin.StateManagement;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;
#endregion

namespace RagadesCubeWin.States
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class RCTestState : RCGameState
    {
        float xRot, yRot;

        RCSceneObject root;
        RCCamera mainCamera;
        RCCube theCube;

        public RCTestState(Game game)
            : base(game)
        {
            xRot = 0;
            yRot = 0;


            input = new RagadesCubeWin.Input.InputManager(game);
            input.Initialize();

            input.AddEvent(Keys.W, Input.Types.EventTypes.Pressed, this.YRotUp);
            input.AddEvent(Keys.S, Input.Types.EventTypes.Pressed, this.YRotDown);
            input.AddEvent(Keys.A, Input.Types.EventTypes.Pressed, this.XRotDown);
            input.AddEvent(Keys.D, Input.Types.EventTypes.Pressed, this.XRotUp);
            
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Construct a scene with a camera, a light, and a cubelet.
            mainCamera = new RCCamera(graphics.GraphicsDevice.Viewport);

            // The local position of the camera is the inverse of the view matrix.
            mainCamera.localTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(10, 10, 10),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0)
                ));

            RCCameraManager.AddCamera(mainCamera, "Main Camera");
            RCCameraManager.SetActiveCamera("Main Camera");

            RCNode rootNode = new RCNode();

            // Set up light node
            RCDirectionalLight lightNode = new RCDirectionalLight(RCRenderManager.DirectionalLightIndex.Light0);

            lightNode.Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            lightNode.Specular = new Vector3(1.0f, 1.0f, 1.0f);

            Vector3 lightDirection = new Vector3(-1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();

            lightNode.Direction = lightDirection;

            rootNode.AddChild(lightNode);

            // Add cuble and camera

            theCube = new RCCube(3, 3, 3);

            lightNode.AddChild(theCube);
            

            rootNode.AddChild(mainCamera);


            // Assign the root node to the class's
            root = rootNode;

            base.Initialize();
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // Load graphics content throughout the scene graph
                root.LoadGraphicsContent(graphics.GraphicsDevice, content);
            }

            base.LoadGraphicsContent(loadAllContent);
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Simple input watching so we can move our cubelet.

            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            yRot += padState.ThumbSticks.Left.X * 0.05f;
            xRot += padState.ThumbSticks.Left.Y * -0.05f;

            input.Update(gameTime);

            //KeyboardState state = Keyboard.GetState();
            //if (state[Keys.S] == KeyState.Down)
            //{
            //    xRot += 0.05f;
            //}
            //if (state[Keys.W] == KeyState.Down)
            //{
            //    xRot -= 0.05f;
            //}
            //if (state[Keys.A] == KeyState.Down)
            //{
            //    yRot -= 0.05f;
            //}
            //if (state[Keys.D] == KeyState.Down)
            //{
            //    yRot += 0.05f;
            //}

            // Rotate cubelet
            theCube.localTrans = Matrix.CreateRotationY(yRot) * Matrix.CreateFromAxisAngle(-RCCameraManager.ActiveCamera.worldTrans.Left, xRot);

            root.UpdateGS(gameTime, true);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            RCRenderManager.DrawScene(root);

            base.Draw(gameTime);
        }

        public void XRotUp()
        {
            xRot += 0.05f;
        }

        public void XRotDown()
        {
            xRot -= 0.05f;
        }

        public void YRotUp()
        {
            yRot -= 0.05f;
        }

        public void YRotDown()
        {
            yRot += 0.05f;
        }
    }
}


