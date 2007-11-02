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
using RagadesCubeWin.GameLogic.InputSchemes;
using RagadesCubeWin.GameLogic;
using RagadesCubeWin.GameLogic.Rules;


#endregion

namespace RagadesCubeWin.States
{
    class RCGuiTestState : RCGameState, IDisposable
    {
       

        RCGUIManager guiManager;
        GuiInputScheme guiInput;
        //RCButton testButton;


        RCButton testButton;
        RCButton toggleButton;

        RCSpinner spinBox;
        
        int timer = 0;

        public RCGuiTestState(Game game)
            : base(game)
        {

        }

        void IDisposable.Dispose()
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
            guiInput = new GuiInputScheme();
            guiInput.Apply(input, guiManager);

            guiScene.Camera.ClearOptions = ClearOptions.DepthBuffer | ClearOptions.Target;

            _sceneManager.AddScene(
                guiScene
                );

            _sceneManager.AddScene(
                new FPScreenScene(graphics.GraphicsDevice.Viewport, Game.Services)
            );


            spinBox = new RCSpinner(100, 100, 100, 100, LucidaFont);
            guiScene.ScreenPane.AddChild(spinBox, 150, 375, 0f);
            spinBox.addSpinItem("No Input", "", "EmptySpinBox", LucidaFont);
            spinBox.addSpinItem("Mouse Input", "Mouse1", "Mouse", LucidaFont);
            spinBox.spinTo("No Input");
//            spinBox.spinUp();
//            spinBox.spinUp();
            spinBox.Accepted += reflectSpinnerValue;
            
        }

        void reflectSpinnerValue()
        {
    
            testButton.buttonText.Text = spinBox.currentKey;
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

            RCGamePlayState gameState = new RCGamePlayState(Game);
            RCGameLogic gameLogic = new RCGameLogic(Game, gameState);

            RCGLKeyboardInputScheme k1 = new RCGLKeyboardInputScheme();
            gameLogic.AddPlayer(k1);

            RCGLGamePadInputScheme gp = new RCGLGamePadInputScheme(PlayerIndex.One);
            gameLogic.AddPlayer(gp);

            //RCGLKeyboardInputScheme k3 = new RCGLKeyboardInputScheme();
            //k3.LeftPressKey = Keys.Right;
            //k3.RightPressKey = Keys.Left;
            //k3.UpPressKey = Keys.Up;
            //k3.DownPressKey = Keys.Down;
            //gameLogic.AddPlayer(k3);

            //RCGLKeyboardInputScheme k2 = new RCGLKeyboardInputScheme();
            //gameLogic.AddPlayer(k2);

            gameLogic.Start(new RCDefaultGameRules(gameLogic));
            gameManager.PushState(gameState);
        }
    }
}
