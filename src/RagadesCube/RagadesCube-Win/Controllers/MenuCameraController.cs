using System;
using System.Collections.Generic;
using System.Text;

using RC.Engine.Animation;
using Microsoft.Xna.Framework;
using RC.Engine.Cameras;

namespace RagadesCube.Controllers
{
    class RCMenuCameraController 
        :RCKeyFrameController<RCPerspectiveCamera>
    {
        public event AnimationCompleteHandler AtDestination;

        enum AnimationState
        {
            FindHome,
            Zooming,
            Unzooming,
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
        const float _zoomDuration = 0.35f;
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
            OnComplete += CompletedMovement;
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

        public void FindHome(float duration)
        {
            _cameraFinalPos = CameraPositions.Home;
            _state = AnimationState.FindHome;

            TranslationMode = InterpolationMode.SmoothStep;
            BeginAnimation(
                _controlledItem.LocalTrans,
                GetZoomedTransform(_cameraFinalPos),
                duration
                );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            
        }

        protected void CompletedMovement()
        {
            switch (_state)
            {
                case AnimationState.ToDestination:
                    ZoomIn();
                    break;
                case AnimationState.Zooming:
                    // Reached the final transform, stop.
                    _state = AnimationState.Stopped;
                    
                    if (AtDestination != null)
                    {
                        AtDestination();
                    }

                    break;
                case AnimationState.Unzooming:
                    GoToDestination();
                    break;

                case AnimationState.FindHome:
                    _state = AnimationState.Stopped;
                    TranslationMode = InterpolationMode.Linear;

                    if (AtDestination != null)
                    {
                        AtDestination();
                    }
                    break;
                case AnimationState.Stopped:
                    break;
                default:
                    break;
            }
        }

        private void ZoomIn()
        {
            _curPosition = _cameraFinalPos;
            // Now we pan in towards the cube.
            _curZoomTrans = GetZoomedTransform(_cameraFinalPos);

            _state = AnimationState.Zooming;
            BeginAnimation(
                _controlledItem.LocalTrans,
                _curZoomTrans,
                _zoomDuration
                );
        }

        private void GoToDestination()
        {
            // Move from to the final destination.
            _state = AnimationState.ToDestination;
            BeginAnimation(
                _controlledItem.LocalTrans,
                _keyFrames[_cameraFinalPos],
                _moveDuration
                );
            _curPosition = _cameraFinalPos;
            
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
                _controlledItem.LocalTrans,
                _keyFrames[_curPosition],
                _zoomDuration
                );
        }
    }
}
