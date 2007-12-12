using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RC.Engine.Animation;
using RC.Engine.GraphicsManagement;


namespace RagadesCube.Controllers
{
    class RCWobbleController: Controller<RCSpatial>
    {

        Vector3 _currentRotation;
        Vector3 _rotationAmplitude;
        Vector3 _period;
        Vector3 _translation;

        Vector3 _pivot;

        float _secondsCount;

        bool _wobble;

        public Vector3 Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public Vector3 RotationAmplitude
        {
            get { return _rotationAmplitude; }
            set { _rotationAmplitude = value; }
        }

        public bool Wobble
        {
            get { return _wobble; }
            set { _wobble = true; }
        }

        public Vector3 LocalPivot
        {
            get { return _pivot; }
            set { _pivot = value; }
        }

        public Vector3 Translation
        {
            get { return _translation; }
            set { _translation = value; }
        }

        public RCWobbleController()
            :base()
        {
            _currentRotation = Vector3.Zero;
            _pivot = Vector3.Zero;

            _secondsCount = 0.0f;

            _wobble = true;
            _isAnimating = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_wobble)
            {
                Vector3 trans;
                Vector3 scale;
                Quaternion rot;

                _controlledItem.LocalTrans.Decompose(
                    out scale,
                    out rot,
                    out trans
                    );

                _secondsCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

                _currentRotation = _rotationAmplitude *
                    new Vector3(
                        _period.X == 0.0f ? 0.0f : (float)Math.Sin(MathHelper.TwoPi / _period.X * _secondsCount),
                        _period.Y == 0.0f ? 0.0f : (float)Math.Sin(MathHelper.TwoPi / _period.Y * _secondsCount),
                        _period.Z == 0.0f ? 0.0f : (float)Math.Sin(MathHelper.TwoPi / _period.Z * _secondsCount)
                     );



                _controlledItem.LocalTrans =
                    Matrix.CreateTranslation(-_pivot) *
                    Matrix.CreateScale(scale) *
                    Matrix.CreateRotationX(_currentRotation.X) *
                    Matrix.CreateRotationY(_currentRotation.Y) *
                    Matrix.CreateRotationZ(_currentRotation.Z) *
                    Matrix.CreateTranslation(_pivot + trans);

            }
                

        }
    }
}
