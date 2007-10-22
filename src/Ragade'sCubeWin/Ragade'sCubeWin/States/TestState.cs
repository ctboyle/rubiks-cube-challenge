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

        RCGUIManager cubeGuiManager;
        RCGUIManager guiManager;
        GuiInputScheme guiInput;
        GuiInputScheme cubeGuiInput;

        RCButton testButton;
        int timer = 0;

        public RCTestState(Game game)
            : base(game)
        {
            xRot = 0;
            yRot = 0;

            inputScheme = new RCTestInputScheme(input);
            inputScheme.Apply(this);

            guiInput = new GuiInputScheme(input);
            cubeGuiInput = new GuiInputScheme(input);
            
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

            RCPane screenPane = new RCPane(
                guiCamera.Width,
                guiCamera.Height,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height
                );

            screenPane.LocalTrans = Matrix.CreateTranslation(
                new Vector3(
                    -(guiCamera.Width / 2.0f + 0.5f),
                    guiCamera.Height / 2.0f + 0.5f,
                    0
                    ));


            IFontManager fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));

            BitmapFont LucidaFont = fontManager.GetFont("Lucida Console");
            BitmapFont RockwellFont = fontManager.GetFont("Rockwell Extra Bold");

            RCText cubeText = new RCText(
                RockwellFont,
                6.0f, 6.0f,
                6, 6
                );

            cubeText.Text = "Ragade's Cube";
            cubeText.Color = Color.White;
            
                      

            float size = RockwellFont.MeasureString(cubeText.Text);
            cubeText.ScaleText(size / 6.0f);

            cubeText.LocalTrans *= Matrix.CreateTranslation(
                new Vector3(-3.0f, 1.0f, 3.05f)
                );
            theCube.AddChild(cubeText);

            
            TestControl testControl = new TestControl(6f, 2f, 510, 150, RockwellFont);
            testControl.LocalTrans *= Matrix.CreateTranslation(
                new Vector3(-3.0f, 3.0f, 3.05f)
                );

            theCube.AddChild(testControl);
            

            testButton = new RCButton(200f, 50f, 200, 50, LucidaFont);
            testButton.buttonText.Color = Color.Red;
            testButton.buttonText.Font = LucidaFont;

            
            

            screenPane.AddChild(testButton, 100, 50, 0f);


            guiRoot = new RCSceneNode();
            guiRoot.AddChild(guiCamera);
            guiRoot.AddChild(screenPane);

            RCScene guiScene = new RCScene(
                    guiRoot,
                    "GUI Camera"
                    );


            guiManager = new RCGUIManager(guiScene);
            guiInput.Apply(guiManager);

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
            Vector3 lightDirection = new Vector3(-1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            lightNode.Direction = lightDirection;

            
            // Add cube and camera
            theCube = new RCCube(3, 3, 3);
            
            cubeCursor = new RCCubeCursor(theCube, Color.DarkRed);
            
            // Add animation controller
            cubeController = new RCCubeController();           
            

            theCube.AttachController(cubeController);
            theCube.AddChild(cubeCursor);
            lightNode.AddChild(theCube);
            rootNode.AddChild(lightNode);
            rootNode.AddChild(mainCamera);

            

            cubeScene = new RCScene(
                    rootNode,
                    "Main Camera"
                    );
            
            cubeGuiManager = new RCGUIManager(cubeScene);
            cubeGuiInput.Apply(cubeGuiManager);

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
            cubeCursor.IsVisible = !cubeController.IsAnimating;

            // Simple input watching so we can move our cubelet.
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            yRot += padState.ThumbSticks.Left.X * 0.05f;
            xRot += padState.ThumbSticks.Left.Y * 0.05f;

            input.Update(gameTime);


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


