#region Using Statements
using System;
using System.Collections.Generic;

using System.Diagnostics;
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
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GUI;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.SoundManagement;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.GUI.Controls.Control_Subclasses;

#endregion

namespace RagadesCubeWin.States
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class RCTestState : RCGameState
    {
        float xRot, yRot;

        RCScene cubeScene;

        RCTestInputScheme inputScheme;
        RCSceneNode guiRoot;
        RCCamera mainCamera;
        RCOrthographicCamera guiCamera;
        RCCube theCube;
        RCCubeController cubeController;
        RCCubeCursor cubeCursor;

        RCGUIManager guiManager;
        GuiInputScheme guiInput;
        GuiInputScheme cubeGuiInput;

       
        RCButton timeButton;

        DateTime timeBegan;
        DateTime currentTime;
        int timer = 0;

        public RCTestState(Game game)
            : base(game)
        {
            xRot = 0;
            yRot = 0;

            inputScheme = new RCTestInputScheme();
            inputScheme.Apply(input, this);

            guiInput = new GuiInputScheme();
            cubeGuiInput = new GuiInputScheme();
            timeBegan = DateTime.MinValue;
        }

        ~RCTestState()
        {
            inputScheme.Unapply();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            CreateCubeScene();
            CreateGuiScene();


            base.Initialize();
        }

        private void CreateGuiScene()
        {
            // Get fonts.
            IFontManager fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));

            BitmapFont LucidaFont = fontManager.GetFont("Lucida Console");
            BitmapFont RockwellFont = fontManager.GetFont("Rockwell Extra Bold");

            RCScreenScene guiScene = new RCScreenScene(
                graphics.GraphicsDevice.Viewport
                );
           

            timeButton = new RCButton(180f, 50f, 180, 50, LucidaFont);
            timeButton.buttonText.Color = Color.Red;
            timeButton.buttonText.Font = LucidaFont;
            guiScene.ScreenPane.AddChild(timeButton, 100, 50, 0f);


            guiManager = new RCGUIManager(guiScene);
            guiInput.Apply(input, guiManager);

            _sceneManager.AddScene(
                guiScene
                );

        }

        private void CreateCubeScene()
        {
            // Create camera

            mainCamera = new RCPerspectiveCamera(graphics.GraphicsDevice.Viewport);

            // The local position of the camera is the inverse of the view matrix.
            mainCamera.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(10, 10, 10),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0)
                ));

            RCCameraManager.AddCamera(mainCamera, "Main Camera");


            // Create objects
            RCSceneNode rootNode = new RCSceneNode();
            

            // Set up light node
            RCDirectionalLight lightNode = new RCDirectionalLight(
                RCRenderManager.DirectionalLightIndex.Light0
                );
            lightNode.Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
            lightNode.Specular = new Vector3(1.0f, 1.0f, 1.0f);

            lightNode.AddChild(lightNode.LightSource);

            lightNode.LightSource.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                Vector3.One,
                Vector3.Zero,
                Vector3.Up
                ));

            
            // Add cube and camera
            theCube = new RCCube(3, 3, 3);
            
            cubeCursor = new RCCubeCursor(theCube, Color.DarkRed);
            
            // Add animation controller
            cubeController = new RCCubeController();


            cubeController.AttachToObject(theCube);
            theCube.AddChild(cubeCursor);
            lightNode.AddChild(theCube);
            rootNode.AddChild(lightNode);
            rootNode.AddChild(mainCamera);

            

            cubeScene = new RCScene(
                    rootNode,
                    "Main Camera"
                    );
            

            _sceneManager.AddScene(
                cubeScene
                );
           


        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {

            base.LoadGraphicsContent(loadAllContent);
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            base.UnloadGraphicsContent(unloadAllContent);
        } 


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (timeBegan == DateTime.MinValue)
            {
                timeBegan = DateTime.Now;
            }
            currentTime = DateTime.Now;
            TimeSpan timeElapsed = (currentTime - timeBegan);
            timeButton.buttonText.Text = (byte)timeElapsed.TotalHours + ":" + (byte)timeElapsed.TotalMinutes + ":" + (byte)timeElapsed.TotalSeconds + ":" + (byte)timeElapsed.TotalMilliseconds;
            timeButton.buttonText.Text = timeButton.buttonText.Text + " (BTN)";
            
            // Simple input watching so we can move our cubelet.

////            timer = 0;//Comment this to make the button change
//            timer++;
//            if(timer>100 && timer<=200)
//            {
//                testButton.buttonText.Text = "Preparing to select...[" + (200-timer) + "]";
//                testButton.Unfocus();
//            }
//            else if(timer>200 && timer<=300)
//            {
//                testButton.buttonText.Text = "Selected.[" + (300-timer) + "]";
//                testButton.Focus();
//            }
//            else if (timer > 300 && timer <= 400)
//            {
//                testButton.buttonText.Text = "Preparing to activate...[" + (400-timer) + "]";
//                testButton.Focus();
//            }
//            else if (timer > 400 && timer <= 500)
//            {
//                testButton.buttonText.Text = "Activated.[" + (500-timer) + "]";
//                testButton.Pressing();
//            }
//            else if (timer > 500 && timer <= 600)
//            {
//                testButton.buttonText.Text = "Preparing to start over...[" + (600-timer) + "]";
//                testButton.Pressing();
//            }
//            else if (timer > 600 )
//            {
//                timer = 0;
//                testButton.buttonText.Text = "Nameless Button";
//                testButton.Unfocus();
//            }

            // Rotate cubelet
            theCube.LocalTrans = Matrix.CreateRotationY(yRot) * Matrix.CreateFromAxisAngle(mainCamera.WorldTrans.Right, xRot);

            base.Update(gameTime);
        }


        public void XRotUp()
        {
            xRot -= 0.05f;
        }

        public void CubeMove(Vector2 pos, Vector2 hov)
        {
            xRot -= pos.Y/20f;
            yRot += pos.X/20f;
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
            Debug.Write("Any key was pressed!", "TestState");
        }

        public void OnRotateUp()
        {
            cubeController.RotateFace(cubeCursor.SelectedFace, RCCube.RotationDirection.CounterClockwise);
        }

        public void OnRotateDown()
        {
            cubeController.RotateFace(cubeCursor.SelectedFace, RCCube.RotationDirection.Clockwise);
        }

        public void OnSelHorizontalFace()
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

        public void OnSelVerticalFace()
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

        public void OnSelOppFace()
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


