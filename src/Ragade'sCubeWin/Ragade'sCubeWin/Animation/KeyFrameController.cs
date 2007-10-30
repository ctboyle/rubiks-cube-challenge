using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GraphicsManagement;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.Animation
{
    public class RCKeyFrameController<CntrlType> 
        : DurationController<CntrlType> where CntrlType : RCSpatial
    {
        public enum InterpolationMode
        {
            Linear,
            SmoothStep
        }

        public enum QuaternionInterploationMode
        {
            Linear,
            Spherical
        }

        protected Vector3 _sourceTrans;
        protected Vector3 _sourceScale;
        protected Quaternion _sourceRot;

        protected Vector3 _destTrans;
        protected Vector3 _destScale;
        protected Quaternion _destRot;

        protected bool _doTranslation;
        protected bool _doScale;
        protected bool _doRotation;

        protected InterpolationMode _scaleMode;
        protected InterpolationMode _translationMode;
        protected InterpolationMode _rotationMode;

        protected QuaternionInterploationMode _rotationCoordMode;

        public InterpolationMode ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        public InterpolationMode TranslationMode
        {
            get { return _translationMode; }
            set { _translationMode = value; }
        }

        public InterpolationMode RotationMode
        {
            get { return _rotationMode; }
            set { _rotationMode = value; }
        }

        public QuaternionInterploationMode RotationCoordinateMode
        {
            get { return _rotationCoordMode; }
            set { _rotationCoordMode = value; }
        }

        public bool DoTranslation
        {
            get { return _doTranslation; }
            set { _doTranslation = value; }
        }

        public bool DoScale
        {
            get { return _doScale; }
            set { _doScale = value; }
        }

        public bool DoRotation
        {
            get { return _doRotation; }
            set { _doRotation = value; }
        }

        public RCKeyFrameController()
            : base()
        {
            _doTranslation = true;
            _doScale = true;
            _doRotation = true;

            RotationMode = InterpolationMode.Linear;
            ScaleMode = InterpolationMode.Linear;
            TranslationMode = InterpolationMode.Linear;

            RotationCoordinateMode = QuaternionInterploationMode.Linear;
        }

        public void BeginAnimation(
            Matrix source,
            Matrix destination,
            float duration
            )
        {
            Begin(duration);

            source.Decompose(
                out _sourceScale,
                out _sourceRot,
                out _sourceTrans
                );

            destination.Decompose(
                out _destScale,
                out _destRot,
                out _destTrans
                );
 
        }

        protected override void UpdateDurationAnimation(
            float percentComplete,
            bool isLastFrame
            )
        {
            Vector3 lerpTrans;
            Vector3 lerpScale;
            Quaternion lerpRot;

            _controlledItem.LocalTrans.Decompose(
                out lerpScale,
                out lerpRot,
                out lerpTrans
                );
           
            
            if (_doRotation)
            {
                float alteredPecentComplete = 0.0f;
                switch (RotationMode)
                {
                    case InterpolationMode.Linear:
                        alteredPecentComplete = percentComplete;
                        break;
                    case InterpolationMode.SmoothStep:
                        // Alter the percent complete to make smooth stepping.
                        alteredPecentComplete = MathHelper.SmoothStep(0.0f, 1.0f, percentComplete);
                        break;
                }

                switch (RotationCoordinateMode)
                {
                    case QuaternionInterploationMode.Linear:
                        lerpRot = Quaternion.Lerp(_sourceRot, _destRot, alteredPecentComplete);
                        break;
                    case QuaternionInterploationMode.Spherical:
                        lerpRot = Quaternion.Slerp(_sourceRot, _destRot, alteredPecentComplete);
                        break;

                }
                
            }
            if (_doScale)
            {
                switch (ScaleMode)
                {
                    case InterpolationMode.Linear:
                        lerpScale = Vector3.Lerp(_sourceScale, _destScale, percentComplete);
                        break;
                    case InterpolationMode.SmoothStep:
                        lerpScale = Vector3.SmoothStep(_sourceScale, _destScale, percentComplete);
                        break;
                }
                
            }

            if ( _doTranslation)
            {
                switch (TranslationMode)
                {
                    case InterpolationMode.Linear:
                        lerpTrans = Vector3.Lerp(_sourceTrans, _destTrans, percentComplete);
                        break;
                    case InterpolationMode.SmoothStep:
                        lerpTrans = Vector3.SmoothStep(_sourceTrans, _destTrans, percentComplete);
                        break;
                }
            }

            _controlledItem.LocalTrans =
                Matrix.CreateScale(lerpScale) *
                Matrix.CreateFromQuaternion(lerpRot) *
                Matrix.CreateTranslation(lerpTrans);
                
        }
           
    }
}
