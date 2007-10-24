using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.Animation;
using Microsoft.Xna.Framework;
using RagadesCubeWin.Cameras;

namespace RagadesCubeWin.States.MainMenu
{
    class RCMenuCameraController 
        :RCKeyFrameController<RCPerspectiveCamera>
    {
        enum AnimationState
        {
            Zooming,
            Unzooming,
            ToHome,
            ToDestination,
            Stopped
        }

        public enum CameraPositions
        {
            Home,
            Left,
            Right,
            Top,
            Bottom
        }

        CameraPositions _cameraFinalPos;
        CameraPositions _curPosition;

        AnimationState _state;

        const float _moveDuration = 1.00f;
        const float _zoomDuration = 0.15f;
        const float _cameraDistance = 14.5f;
        const float _zoomDistance = _cameraDistance - 10.15f;
        
        Matrix _curZoomTrans;

        Dictionary<CameraPositions, Matrix> _keyFrames;

        public CameraPositions CurrentPosition
        {
            get { return _curPosition; }
        }

        public RCMenuCameraController()
            : base()
        {
            _keyFrames = new Dictionary<CameraPositions, Matrix>();
            CreateKeyFrames();

        }

        private void CreateKeyFrames()
        {
            _keyFrames[CameraPositions.Home] = Matrix.CreateTranslation(
                new Vector3(0.0f, 0.0f, _cameraDistance)
                );

            _keyFrames[CameraPositions.Right] =
                _keyFrames[CameraPositions.Home] * 
                Matrix.CreateRotationY(MathHelper.PiOver2);

            _keyFrames[CameraPositions.Left] =
                _keyFrames[CameraPositions.Home] *
                Matrix.CreateRotationY(-MathHelper.PiOver2);

            _keyFrames[CameraPositions.Top] =
                _keyFrames[CameraPositions.Home] *
                Matrix.CreateRotationX(-MathHelper.PiOver2);

            _keyFrames[CameraPositions.Bottom] =
                _keyFrames[CameraPositions.Home] *
                Matrix.CreateRotationX(MathHelper.PiOver2);

        }

        public void GoToPosition(CameraPositions position)
        {
            if (!IsAnimating)
            {
                // Begin animation state machine.
                _state = AnimationState.Unzooming;

                _cameraFinalPos = position;

                UnZoom();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Remove interpolated scaling.

            Vector3 scale;
            Vector3 trans;
            Quaternion q;
            _parentSceneObject.LocalTrans.Decompose(
                out scale,
                out q,
                out trans
                );

            _parentSceneObject.LocalTrans =
                Matrix.CreateFromQuaternion(q) *
                Matrix.CreateTranslation(trans);
        }

        protected override void OnComplete()
        {
            switch (_state)
            {
                case AnimationState.ToHome:
                    // Now go to destination.
                    GoToDestination();
                    break;

                case AnimationState.ToDestination:
                    ZoomIn();
                    break;
                case AnimationState.Zooming:
                    // Reached the final transform, stop.
                    _state = AnimationState.Stopped;
                    break;
                case AnimationState.Unzooming:
                    GoToHome();
                    break;
                case AnimationState.Stopped:
                    break;
            }
        }

        private void GoToHome()
        {
            // Just unzoomed, go to home position.


            // No need to move just stay here, zoom back in.
            if (_curPosition == _cameraFinalPos)
            {
                ZoomIn();
            }
            else if (_curPosition == CameraPositions.Home)
            {
                 GoToDestination();
            }
            else
            {
                _state = AnimationState.ToHome;
                BeginAnimation(
                    _parentSceneObject.LocalTrans,
                    _keyFrames[CameraPositions.Home],
                    _moveDuration
                    );
            }
        }

        private void ZoomIn()
        {
            _curPosition = _cameraFinalPos;
            // Now we pan in towards the cube.
            _curZoomTrans = GetZoomedTransform(_cameraFinalPos);

            _state = AnimationState.Zooming;
            BeginAnimation(
                _parentSceneObject.LocalTrans,
                _curZoomTrans,
                _zoomDuration
                );
        }

        private void GoToDestination()
        {
            if (_cameraFinalPos == CameraPositions.Home)
            {
                // We should already be at home.
                ZoomIn();
            }
            else
            {
                // Move from home to the final destination.
                _state = AnimationState.ToDestination;
                BeginAnimation(
                    _parentSceneObject.LocalTrans,
                    _keyFrames[_cameraFinalPos],
                    _moveDuration
                    );
                _curPosition = _cameraFinalPos;
            }
        }

        private Matrix GetZoomedTransform(CameraPositions position)
        {
            Vector3 dir = _keyFrames[position].Forward;
            dir.Normalize();

            return _keyFrames[position] * Matrix.CreateTranslation(
                dir * _zoomDistance
                );
                
        }

        private void UnZoom()
        {
            _state = AnimationState.Unzooming;
            BeginAnimation(
                _parentSceneObject.LocalTrans,
                _keyFrames[_curPosition],
                _zoomDuration
                );
        }
    }
}
