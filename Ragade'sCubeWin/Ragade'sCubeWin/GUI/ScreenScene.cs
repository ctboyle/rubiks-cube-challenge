using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.Cameras;
using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.SceneManagement;

using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GraphicsManagement;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GUI
{
    public class RCScreenScene :RCScene
    {
        private static int _sceneID;
        private RCPane _screenPane;

        public float CameraZDepth
        {
            get 
            { 
                return Camera.LocalTrans.Translation.Z;
            }

            set
            {
                Camera.LocalTrans = Matrix.CreateTranslation(
                    new Vector3(0.0f, 0.0f, value)
                    );
            }
        }

        public RCPane ScreenPane
        {
            get { return _screenPane; }
        }

        public RCCamera Camera
        {
            get
            {
                return RCCameraManager.GetCamera(
                   _cameraLabel
                   );
            }

        }

        public RCScreenScene(Viewport sceneViewport)
            : base(null, "")
        {
            RCOrthographicCamera guiCamera = new RCOrthographicCamera(
                sceneViewport
                );

            guiCamera.Width = (float)sceneViewport.Width;
            guiCamera.Height = (float)sceneViewport.Height;

            // Set defualt behavior of the camera.
            guiCamera.ClearOptions =
                ClearOptions.DepthBuffer;
            
            // Default camera position
            guiCamera.LocalTrans = Matrix.Invert(Matrix.CreateLookAt(
                new Vector3(0, 0, 10),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0)
                ));

            // Add camera to camera manager
            _cameraLabel = "GUI Scene Camera - " + _sceneID.ToString();
            RCCameraManager.AddCamera(guiCamera, _cameraLabel);

            // Create a pane who's size is the size of the
            // specified viewport.
            _screenPane = new RCPane(
                guiCamera.Width,
                guiCamera.Height,
                sceneViewport.Width,
                sceneViewport.Height
                );

            // Position the pane to be in the upper left corner of the
            // view frustum and the size of the near plane.
            // But align it so that the points fall in the center of the
            // screen pixels.
            _screenPane.LocalTrans = Matrix.CreateTranslation(
                new Vector3(
                    -(guiCamera.Width / 2.0f + 0.5f),
                    guiCamera.Height / 2.0f + 0.5f,
                    0
                    ));

            // Create a sceneNode for the root of the scene.
            RCSceneNode sceneRoot = new RCSceneNode();
            // Add children, camera and screen pane.
            sceneRoot.AddChild(_screenPane);
            sceneRoot.AddChild(guiCamera);

            _sceneRoot = sceneRoot;

            _sceneID++;
        }
    }
}
