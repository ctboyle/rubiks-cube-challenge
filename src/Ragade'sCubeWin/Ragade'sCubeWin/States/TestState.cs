#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;


using RagadesCubeWin.Animation.Controllers;
using RagadesCubeWin.StateManagement;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input.Types;

using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.GUI.Primitives;
#endregion

namespace RagadesCubeWin.States
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class RCTestState : RCGameState
    {
        float xRot, yRot;

        RCSpatial root;
        RCSceneNode guiRoot;
        RCCamera mainCamera;
        RCOrthographicCamera guiCamera;
        RCCube theCube;
        RCCubeController cubeController;
        RCCubeCursor cubeCursor;

        RCQuad testQuad;

        public RCTestState(Game game)
            : base(game)
        {
            xRot = 0;
            yRot = 0;

            input = new InputManager(game);
            input.Initialize();

            #region GamePadWatcher
            IWatcher watchplayer1 = new Input.Watchers.XBox360GamePad(PlayerIndex.One);

            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.X, EventTypes.OnDown, OnSelHorizontalFace));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.A, EventTypes.OnDown, OnSelVerticalFace));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.B, EventTypes.OnDown, OnSelOppFace));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DUP, EventTypes.OnDown, OnRotateUp));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DDOWN, EventTypes.OnDown, OnRotateDown));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.LEFTANALOG, EventTypes.Leaned, CubeMove));
           
            input.AddWatcher(watchplayer1);

            #endregion

            #region Keyboardwatcher

            IWatcher watchkeyboard = new Input.Watchers.Keyboard();

            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, YRotUp));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.D, EventTypes.Pressed, YRotDown));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.W, EventTypes.Pressed, XRotUp));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.S, EventTypes.Pressed, XRotDown));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageUp, EventTypes.Pressed, OnRotateUp));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageDown, EventTypes.Pressed, OnRotateDown));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Up, EventTypes.OnUp, OnSelHorizontalFace));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Down, EventTypes.OnUp, OnSelVerticalFace));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Left, EventTypes.OnUp, OnSelOppFace));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Right, EventTypes.OnUp, OnSelOppFace));

            watchkeyboard.WatchEvent(new KeyboardEvent(EventTypes.Pressed , XXX));

            input.AddWatcher(watchkeyboard);

            #endregion
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            mainCamera = new RCPerspectiveCamera(graphics.GraphicsDevice.Viewport);


            // The local position of the camera is the inverse of the view matrix.
            mainCamera.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(10, 10, 10),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0)
                ));

            RCCameraManager.AddCamera(mainCamera, "Main Camera");
            RCCameraManager.SetActiveCamera("Main Camera");


            guiCamera = new RCOrthographicCamera(graphics.GraphicsDevice.Viewport);
            guiCamera.Width = (float)graphics.GraphicsDevice.Viewport.Width;
            guiCamera.Height = (float)graphics.GraphicsDevice.Viewport.Height;

            guiCamera.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(0, 0, 10),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0)
                ));

            guiCamera.ClearScreen = false;

            RCCameraManager.AddCamera(guiCamera, "GUI Camera");

            RCSceneNode rootNode = new RCSceneNode();

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

            cubeController = new RCCubeController();

            // Add animation controller
            theCube.AttachController(cubeController);

            cubeCursor = new RCCubeCursor(theCube, Color.DarkRed);
            theCube.AddChild(cubeCursor);


            lightNode.AddChild(theCube);


            rootNode.AddChild(mainCamera);


            // Assign the root node to the class's
            root = rootNode;



            RCPane screenPane = new RCPane(
                guiCamera.Width,
                guiCamera.Height,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height
                );

            testQuad = new RCQuad(100,40,100,40);

            

            screenPane.LocalTrans = Matrix.CreateTranslation(
                new Vector3(
                    -guiCamera.Width/2.0f,
                    guiCamera.Height/2.0f,
                    0
                    ));
            
            screenPane.AddChild(testQuad, 400, 300, 0.0f);


            guiRoot = new RCSceneNode();
            guiRoot.AddChild(guiCamera);
            guiRoot.AddChild(screenPane);





            base.Initialize();
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // Load graphics content throughout the scene graph
                root.LoadGraphicsContent(graphics.GraphicsDevice, content);
                guiRoot.LoadGraphicsContent(graphics.GraphicsDevice, content);
                testQuad.Image = content.Load<Texture2D>(
                    "Content\\Textures\\GuiTest"
                    );
            }

            base.LoadGraphicsContent(loadAllContent);
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            cubeCursor.IsVisible = !cubeController.IsAnimating;

            // Simple input watching so we can move our cubelet.
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            yRot += padState.ThumbSticks.Left.X * 0.05f;
            xRot += padState.ThumbSticks.Left.Y * 0.05f;

            input.Update(gameTime);

           
            // Rotate cubelet
            theCube.LocalTrans = Matrix.CreateRotationY(yRot) * Matrix.CreateFromAxisAngle(mainCamera.WorldTrans.Right, xRot);
            
            root.UpdateGS(gameTime, true);
            guiRoot.UpdateGS(gameTime, true);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            RCCameraManager.SetActiveCamera("Main Camera");
            RCRenderManager.DrawScene(root);

            RCCameraManager.SetActiveCamera("GUI Camera");
            RCRenderManager.DrawScene(guiRoot);

            
            

            base.Draw(gameTime);
        }

        public void XRotUp()
        {
            xRot -= 0.05f;
        }

        public void CubeMove(Vector2 pos, Vector2 hov)
        {
            xRot += pos.X/200;
            yRot += pos.Y/200;
        }

        public void XRotDown()
        {
            xRot += 0.05f;
        }

        public void YRotUp()
        {
            yRot -= 0.05f;
        }

        public void YRotDown()
        {
            yRot += 0.05f;
        }

        public void XXX(Keys key)
        {
                YRotUp();
           
        }

        private void OnRotateUp()
        {
            cubeController.RotateFace(cubeCursor.SelectedFace, RCCube.RotationDirection.CounterClockwise);
        }

        private void OnRotateDown()
        {
            cubeController.RotateFace(cubeCursor.SelectedFace, RCCube.RotationDirection.Clockwise);
        }

        private void OnSelHorizontalFace()
        {
            switch (cubeCursor.SelectedFace)
            {
                case RCCube.FaceSide.Top:
                case RCCube.FaceSide.Bottom:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Front;
                    break;
                case RCCube.FaceSide.Front:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Left;
                    break;
                case RCCube.FaceSide.Back:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Right;
                    break;
                case RCCube.FaceSide.Left:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Back;
                    break;
                case RCCube.FaceSide.Right:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Front;
                    break;
            }
        }

        private void OnSelVerticalFace()
        {
            switch (cubeCursor.SelectedFace)
            {
                case RCCube.FaceSide.Top:
                case RCCube.FaceSide.Bottom:
                    CursorSectionSwitch();
                    break;
                case RCCube.FaceSide.Front:
                case RCCube.FaceSide.Back:
                case RCCube.FaceSide.Left:
                case RCCube.FaceSide.Right:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Top;
                    break;
            }
        }

        private void OnSelOppFace()
        {
            switch (cubeCursor.SelectedFace)
            {
                case RCCube.FaceSide.Top:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Bottom;
                    break;
                case RCCube.FaceSide.Front:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Back;
                    break;
                case RCCube.FaceSide.Bottom:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Top;
                    break;
                case RCCube.FaceSide.Back:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Front;
                    break;
                case RCCube.FaceSide.Left:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Right;
                    break;
                case RCCube.FaceSide.Right:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Left;
                    break;
            }
        }

        private void CursorSectionSwitch()
        {
            switch (cubeCursor.SelectedFace)
            {
                case RCCube.FaceSide.Top:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Bottom;
                    break;
                case RCCube.FaceSide.Front:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Back;
                    break;
                case RCCube.FaceSide.Bottom:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Top;
                    break;
                case RCCube.FaceSide.Back:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Front;
                    break;
                case RCCube.FaceSide.Left:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Right;
                    break;
                case RCCube.FaceSide.Right:
                    cubeCursor.SelectedFace = RCCube.FaceSide.Left;
                    break;
            }
        }
    }
}


