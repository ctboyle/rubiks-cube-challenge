using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GraphicsManagement;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.Animation
{
    public abstract class RCKeyFrameController<CntrlType> 
        : Controller<CntrlType> where CntrlType : RCSpatial
    {


        bool _isAnimating;

        Matrix _sourceTransform;
        Matrix _destTransform;

        float _currentTime;
        float _animationDuration;

        public RCKeyFrameController()
            : base()
        {
            _isAnimating = false;

        }

        public void BeginAnimation(
            Matrix source,
            Matrix destination,
            float duration
            )
        {
            _sourceTransform = source;
            _destTransform = destination;
            _animationDuration = duration;
            _currentTime = 0.0f;
            _isAnimating = true;
        }
 
        public override bool IsAnimating
        {
            get { return _isAnimating; }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (IsAnimating)
            {
                bool fEnd = false;

                _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_currentTime >= _animationDuration)
                {
                    _currentTime = _animationDuration;
                    fEnd = true;
                }

                float percentComplete = _currentTime / _animationDuration;

                _parentSceneObject.LocalTrans = Matrix.Lerp(
                    _sourceTransform,
                    _destTransform,
                    percentComplete
                    );

                if (fEnd)
                {
                    _isAnimating = false;
                    OnComplete();
                }
            }


        }

        

        protected abstract void OnComplete();

    }
}
