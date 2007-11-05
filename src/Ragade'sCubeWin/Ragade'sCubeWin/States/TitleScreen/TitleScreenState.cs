using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.States.MenuCubeState;
using RagadesCubeWin.States.MainMenu;
using Microsoft.Xna.Framework;
using RagadesCubeWin.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.States.Controllers;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Animation;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.States.MenuCubeState.CubeMenus;
using RagadesCubeWin.GUI;

namespace RagadesCubeWin.States.TitleScreen
{
    class RCTitleScreenState : RCMenuCubeState
    {
        private const float _cameraDistance = 30.0f;
        private TitleScreenInputScheme _inputScheme;

        private RCScreenScene _screenScene; 
        private RCQuad _titleImage;

        SpinController cubeSpin;
        
        ScaleController cubeScale;
        RCWobbleController cubeWobble;

        bool _transitioning;

        public RCTitleScreenState(
            Game game
            )
            : base(game)
        {
            _inputScheme = new TitleScreenInputScheme();
            _inputScheme.Apply(input, this);
        }

        public override void Initialize()
        {
            
            InitializeCube();
            //_menuScene.Camera.ClearScreen = tr;
            InitializeScreenScene();
            
            base.Initialize();
            _sceneManager.AddScene(_screenScene);
        }

        private void InitializeScreenScene()
        {
            _screenScene = new RCScreenScene(graphics.GraphicsDevice.Viewport);

            _titleImage = new RCQuad(1, 1, 800, 600);
            _screenScene.ScreenPane.AddChild(
                _titleImage,
                0, 0, 0
                );
            _screenScene.Camera.ClearScreen = false;
            
            

            
        }

        private void InitializeCube()
        {
            
            _menuScene.Camera.LocalTrans = Matrix.CreateTranslation(
                new Vector3(0.0f, 0.0f, _cameraDistance)
                );

            ScaleController cubeScale = new ScaleController();
            cubeScale.AttachToObject(_menuScene.Cube);
            
            cubeScale.BeginAnimation(
                new Vector3(0.1f, 0.1f, 0.1f),
                Vector3.One,
                2.0f
                );

            cubeScale.OnComplete += delegate() { _menuScene.Cube.RemoveController(cubeScale); };
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            _titleImage.Image = content.Load<Texture2D>("Content\\Textures\\TitleImage2");

            base.LoadGraphicsContent(loadAllContent);
        }

        private void StartEnterAnimation()
        {
            _transitioning = true;

            RCWobbleController wobble =
                (RCWobbleController)_menuScene.Cube.GetController<RCWobbleController>();

            if (wobble != null)
            {
                _menuScene.Cube.RemoveController(wobble);
            }

            _menuScene.CameraController.BeginAnimation(
                _menuScene.Camera.LocalTrans,
                Matrix.CreateTranslation(
                    new Vector3(0.0f, 0.0f, _cameraDistance)
                    ),
                2.0f
                );

            _menuScene.CameraController.OnComplete += EndEnterAnimation;

            cubeSpin = new SpinController(
                new Vector3(1.0f, 1.0f, 0.3f),
                MathHelper.ToRadians(24)
                );
            cubeSpin.AttachToObject(_menuScene.Cube);
            

            cubeSpin.Enabled = true;
        }

        private void EndEnterAnimation()
        {
            _transitioning = false;
            _menuScene.CameraController.OnComplete -= EndEnterAnimation;
        }

        private void EndLeaveAnimation()
        {
            gameManager.PushState(new RCMainMenu(Game));
            _transitioning = false;
        }

        private void OnLeaveState()
        {
            _menuScene.Cube.RemoveController(cubeSpin);

            RCWobbleController cubeWobble = new RCWobbleController();

            cubeWobble.Period = 2 * new Vector3(5.0f, 6.4f, 7.7f);
            cubeWobble.RotationAmplitude = new Vector3(
                MathHelper.ToRadians(2.0f),
                MathHelper.ToRadians(2.0f),
                MathHelper.ToRadians(2.0f)
                );

            cubeWobble.AttachToObject(_menuScene.Cube);
            
        }

        protected internal override void StateChanged(RCGameState newState, RCGameState oldState)
        {
            if (newState == this)
            {
                StartEnterAnimation();
            }
            else if (oldState == this)
            {
                OnLeaveState();
            }
            base.StateChanged(newState, oldState);
        }

        public void StartLeaveAnimation()
        {
            if (!_transitioning)
            {
                _transitioning = true;

                // Stop spin
                SpinController spinControl =
                    (SpinController)_menuScene.Cube.GetController<SpinController>();

                spinControl.Enabled = false;

                // Shrink away 2D title items.

                // Orient Cube correctly and push the menu state on completion.
                RCKeyFrameController<RCSpatial> cubeOrienter =
                    new RCKeyFrameController<RCSpatial>();
                cubeOrienter.AttachToObject(_menuScene.Cube);

                cubeOrienter.RotationMode = RCKeyFrameController<RCSpatial>.InterpolationMode.SmoothStep;
                cubeOrienter.BeginAnimation(
                    _menuScene.Cube.LocalTrans,
                    Matrix.Identity,
                    2.0f
                    );

                cubeOrienter.OnComplete += EndLeaveAnimation;

                _menuScene.CameraController.FindHome(1.0f);

            }            
        }
         
        

    }
    

    

}
