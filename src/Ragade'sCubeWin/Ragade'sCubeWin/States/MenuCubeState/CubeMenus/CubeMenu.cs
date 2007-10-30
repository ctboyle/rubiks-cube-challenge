using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.States.MainMenu;
using RagadesCubeWin.GUI;
using RagadesCubeWin.StateManagement;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.SceneManagement;




namespace RagadesCubeWin.States.MenuCubeState.CubeMenus
{
    abstract class  RCCubeMenu : RCMenuCubeState
    {
        protected RCMenuCameraController.CameraPositions _menuPos;

        protected RCPane _menuPane;
        protected RCText _titleText;

        protected RCGUIManager _guiManager;
        protected GuiInputScheme _guiInput;
        protected IFontManager _fontManager;
        protected CubeMenuInputScheme _inputScheme;

        public RCMenuCameraController.CameraPositions Position
        {
            get { return _menuPos; }
        }

        public RCCubeMenu(Game game)
            :base(game)
        {
            _guiInput = new GuiInputScheme();
            _fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));
            _inputScheme = new CubeMenuInputScheme();
            _inputScheme.Apply(input, this);
        }

        public bool AcceptInput
        {
            get { return !_menuScene.CameraController.IsAnimating; }
        }

        public override void Initialize()
        {
            CreateDefaultGuiElements();

            ConstructGuiElements();

            _guiManager = new RCGUIManager(new RCScene(_menuPane, _menuScene.SceneCameraLabel));
            _guiInput.Apply(input, _guiManager);
            

            base.Initialize();
        }

        private void CreateDefaultGuiElements()
        {
            BitmapFont extraLargeFont = _fontManager.GetFont("Ragade's Cube Extra Large");

            // Make a pane to be fitted on the side of the cube.
            _menuPane = new RCPane(6.0f, 6.0f, 600, 600);
            _titleText = new RCText(
                extraLargeFont,
                600, 20,
                600, 20
                );

            _titleText.CenterText = true;

            _menuPane.AddChild(_titleText, 0, 50, 0);
            PositionPane();

        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            _menuPane.LoadGraphicsContent(graphics.GraphicsDevice, content);
            base.LoadGraphicsContent(loadAllContent);
        }

        protected void PositionPane()
        {
            Matrix homePos = Matrix.CreateTranslation(
                new Vector3(-3.0f, 3.0f, 3.10f)
                );

            switch (_menuPos)
            {
                case RCMenuCameraController.CameraPositions.Home:
                    _menuPane.LocalTrans = homePos;
                    break;
                case RCMenuCameraController.CameraPositions.Top:
                    _menuPane.LocalTrans = homePos * Matrix.CreateRotationX(-MathHelper.PiOver2);
                    break;
                case RCMenuCameraController.CameraPositions.Bottom:
                    _menuPane.LocalTrans = homePos * Matrix.CreateRotationX(MathHelper.PiOver2);
                    break;
                case RCMenuCameraController.CameraPositions.Left:
                    _menuPane.LocalTrans = homePos * Matrix.CreateRotationY(-MathHelper.PiOver2);
                    break;
                case RCMenuCameraController.CameraPositions.Right:
                    _menuPane.LocalTrans = homePos * Matrix.CreateRotationY(MathHelper.PiOver2);
                    break;
            }
        }

        /// <summary>
        /// Happens when a state is pushed or popped.
        /// 
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        protected internal override void StateChanged(
            RCGameState newState,
            RCGameState oldState
            )
        {
            if (newState == this)
            {
                if (oldState is RCCubeMenu)
                {
                    OnEnter((RCCubeMenu)oldState);
                }
                else
                {
                    OnEnter(null);
                }
            }
            else if (oldState == this && !(newState is RCCubeMenu))
            {
                OnLeave();
            }
     
            base.StateChanged(newState, oldState);
        }

        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        /// <param name="previous"></param>
        public void OnEnter(RCCubeMenu previous)
        {
            // Turn on the menu pane for this menu.
            _menuScene.Cube.AddChild(_menuPane);

            // Start the the entering animation animation
            BeginEnterAnimation();

            // Tell the previous cube state that the animation is complete.
            if (previous != null)
            {
                _menuScene.CameraController.AtDestination += previous.OnLeave;

            }
        }

        private void BeginEnterAnimation()
        {
            // If we are not already at the menu position we are going to,
            // go there.
            if (_menuScene.CameraController.CurrentPosition != _menuPos)
            {
                // Turn off input while animating.
                _guiManager.AcceptInput = false;

                // Initiate the animation.
                _menuScene.CameraController.GoToPosition(_menuPos);

                //  Ensure input gets turned back on after the anmination is done.
                _menuScene.CameraController.AtDestination +=
                    EndEnterAnimation;
            }

            
        }

        private void EndEnterAnimation()
        {
            _guiManager.AcceptInput = true;
            _menuScene.CameraController.AtDestination -= EndEnterAnimation;
        }

        public void OnLeave()
        {
            _menuScene.Cube.RemoveChild(_menuPane);
            _menuScene.CameraController.AtDestination -= OnLeave;
        }


        public void ExitState()
        {
            if (!_menuScene.CameraController.IsAnimating)
            {
                gameManager.PopState();
            }
        }

        protected abstract void ConstructGuiElements();


    }
}
