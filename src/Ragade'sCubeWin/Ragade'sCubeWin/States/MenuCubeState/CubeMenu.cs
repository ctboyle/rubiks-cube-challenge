using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GUI.Panes;
using Microsoft.Xna.Framework;
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
            _guiInput = new GuiInputScheme(input);
            _fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));
            _inputScheme = new CubeMenuInputScheme(input);
            _inputScheme.Apply(this);
        }

        public override void Initialize()
        {
            CreateDefaultGuiElements();

            ConstructGuiElements();

            _guiManager = new RCGUIManager(new RCScene(_menuPane, _menuScene.SceneCameraLabel));
            _guiInput.Apply(_guiManager);

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


        public void OnEnter(RCCubeMenu previous)
        {
            _menuScene.Cube.AddChild(_menuPane);
            if (_menuScene.CameraController.CurrentPosition != _menuPos)
            {
                _menuScene.CameraController.GoToPosition(_menuPos);
            }
            
            if (previous != null)
            {
                _menuScene.CameraController.AtDestination += previous.OnLeave;
            }
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
