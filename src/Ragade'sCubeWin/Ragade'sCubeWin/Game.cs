#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;
#endregion

namespace RagadesCubeWin
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RagadesCube : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        float xRot, yRot;

        RCSceneObject root;
        RCCamera mainCamera;
        RCCublet cubelet;

        public RagadesCube()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);

            xRot = 0;
            yRot = 0;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize the rendermanager
            RenderManager.Initialize(graphics.GraphicsDevice);


            // Construct a scene with a camera, a light, and a cubelet.
            mainCamera = new RCCamera(graphics.GraphicsDevice.Viewport);

            // The local position of the camera is the inverse of the view matrix.
            mainCamera.localTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(4, 4, 4), 
                new Vector3(0, 0, 0), 
                new Vector3(0, 1, 0)
                ));

            RCCameraManager.AddCamera(mainCamera, "Main Camera");
            RCCameraManager.SetActiveCamera("Main Camera");

            RCNode rootNode = new RCNode();

            // Set up light node
            RCDirectionalLight lightNode = new RCDirectionalLight(RenderManager.DirectionalLightIndex.Light0);

            lightNode.Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            lightNode.Specular = new Vector3(1.0f, 1.0f, 1.0f); 

            Vector3 lightDirection = new Vector3(-1.0f,-1.0f,-1.0f);
            lightDirection.Normalize();

            lightNode.Direction = lightDirection;

            rootNode.AddChild(lightNode);


            // Add cublet and camera

            cubelet = new RCCublet();

            lightNode.AddChild(cubelet);
            
            rootNode.AddChild(mainCamera);


            // Assign the root node to the class's
            root = rootNode;

            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                root.LoadGraphicsContent(graphics.GraphicsDevice, content);
            }

            // TODO: Load any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                // TODO: Unload any ResourceManagementMode.Automatic content
                content.Unload();
            }

            // TODO: Unload any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Simple input watching so we can move our cubelet.
            KeyboardState state = Keyboard.GetState();
            if (state[Keys.S] == KeyState.Down)
            {
                xRot -= 0.05f;
            }
            if (state[Keys.W] == KeyState.Down)
            {
                xRot += 0.05f;
            }
            if (state[Keys.A] == KeyState.Down)
            {
                yRot -= 0.05f;
            }
            if (state[Keys.D] == KeyState.Down)
            {
                yRot += 0.05f;
            }

            // Rotate cubelet
            cubelet.localTrans =   Matrix.CreateRotationX(xRot) * Matrix.CreateRotationY(yRot);

            root.UpdateGS(gameTime, true);

            RenderManager.DrawScene(root);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);          

            root.Draw(graphics.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
