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
    class RCGuiTestState : RCGameState
    {
        float xRot, yRot;

        RCScene cubeScene;



        RCGUIManager guiManager;
        GuiInputScheme guiInput;


        RCButton testButton;
        RCButton toggleButton;
        int timer = 0;

        public RCGuiTestState(Game game)
            : base(game)
        {

        }

        ~RCGuiTestState()
        {

            if (guiInput != null)
            {
                guiInput.Unapply();
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
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

            for (int iRows = 50; iRows < 600; iRows += 200)
            {
                for (int iCols = 50; iCols < 350; iCols += 50)
                {

                    TestControl testControl = new TestControl(6f, 2f, 210, 50, LucidaFont);
                    testControl.Text = "Move Around";
                    guiScene.ScreenPane.AddChild(testControl, iRows, iCols, 0f);

                }
            }

            TestControl testStateControl = new TestControl(6f, 2f, 210, 50, LucidaFont);
            testStateControl.Text = "Test State...";
            guiScene.ScreenPane.AddChild(testStateControl, 100, 450, 0f);


            testStateControl.Accepted+= PushTestState;
            



            testButton = new RCButton(200f, 50f, 200, 50, LucidaFont);
            testButton.buttonText.Color = Color.Red;
            testButton.buttonText.Font = LucidaFont;
            //testButton.AfterPressedAndReleased += PushTestState;
            testButton.AfterPressedAndReleased += flipToggleButton;
            guiScene.ScreenPane.AddChild(testButton, 500, 450, 0f);

            toggleButton = new RCButton(200f, 50f, 200, 50, LucidaFont);
            toggleButton.buttonText.Color = Color.Blue;
            toggleButton.buttonText.Text = "Active / INACTIVE";
            guiScene.ScreenPane.AddChild(toggleButton, 500, 400, 0f);


            guiManager = new RCGUIManager(guiScene);
            guiInput = new GuiInputScheme(input);
            guiInput.Apply(guiManager);

            guiScene.Camera.ClearOptions = ClearOptions.DepthBuffer | ClearOptions.Target;

            _sceneManager.AddScene(
                guiScene
                );

        }

        void flipToggleButton()
        {
            if (toggleButton.isBeingPushed)
            { toggleButton.UnPressingInternalLogic(); }
            else
            { toggleButton.PressingInternalLogic(); }
        }
        

        void PushTestState()
        {
            gameManager.PushState(new RCTestState(this.Game));
        }
    }
}