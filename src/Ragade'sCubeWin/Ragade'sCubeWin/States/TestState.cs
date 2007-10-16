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
        RCCamera mainCamera;
        RCCube theCube;
        RCCubeController cubeController;
        RCCubeCursor cubeCursor;

        public RCTestState(Game game)
            : base(game)
        {
            xRot = 0;
            yRot = 0;

            input = new InputManager(game);
            input.Initialize();

            #region GamePadWatcher
            IWatcher watchplayer1 = new Input.Watchers.XBox360GamePad(PlayerIndex.One);

            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.X, EventTypes.Tapped, OnSelHorizontalFace));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.A, EventTypes.Tapped, OnSelVerticalFace));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.B, EventTypes.Tapped, OnSelOppFace));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DUP, EventTypes.Tapped, OnRotateUp));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DDOWN, EventTypes.Tapped, OnRotateDown));
            watchplayer1.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.LEFTANALOG, EventTypes.Leaned, CubeMove));
           
            input.AddWatcher(watchplayer1);

            #endregion

            #region Keyboardwatcher

            IWatcher watchkeyboard = new Input.Watchers.Keyboard();

            KeyboardEvent pressLShift = new KeyboardEvent(Keys.LeftShift, EventTypes.Pressed, YRotUp);
            KeyboardEvent pressRShift = new KeyboardEvent(Keys.LeftShift, EventTypes.Released, YRotDown);

            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, pressLShift));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, pressRShift));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.W, EventTypes.Pressed, XRotUp));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.S, EventTypes.Pressed, XRotDown));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageUp, EventTypes.Pressed, OnRotateUp));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.PageDown, EventTypes.Pressed, OnRotateDown));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Up, EventTypes.Tapped, OnSelHorizontalFace));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Down, EventTypes.Tapped, OnSelVerticalFace));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Left, EventTypes.Tapped, OnSelOppFace));
            watchkeyboard.WatchEvent(new KeyboardEvent(Keys.Right, EventTypes.Tapped, OnSelOppFace));

            input.AddWatcher(watchkeyboard);

            #endregion
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

            cubeController = new RCCubeController();

            // Add animation controller
            theCube.AttachController(cubeController);

            cubeCursor = new RCCubeCursor(theCube, Color.DarkRed);
            theCube.AddChild(cubeCursor);


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
            cubeCursor.IsVisible = !cubeController.IsAnimating;

            // Simple input watching so we can move our cubelet.
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            yRot += padState.ThumbSticks.Left.X * 0.05f;
            xRot += padState.ThumbSticks.Left.Y * -0.05f;

            input.Update(gameTime);

            // Rotate cubelet
            theCube.localTrans = Matrix.CreateRotationY(yRot) * Matrix.CreateFromAxisAngle(-RCCameraManager.ActiveCamera.worldTrans.Left, -xRot);
            
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

        public void CubeMove(Vector2 pos, Vector2 hov)
        {
            xRot += pos.X/200;
            yRot += pos.Y/200;
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


