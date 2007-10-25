using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.Animation;
using RagadesCubeWin.SceneObjects;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.States.MainMenu
{
    class MainMenuCubeController: Controller<RCCube>
    {
        bool _isAnimating;

        Vector3 _currentRotation;
        Vector3 _rotationAmplitude;
        Vector3 _period;

        float _secondsCount;

        bool _wobble;

        public bool Wobble
        {
            get { return _wobble; }
            set { _wobble = true; }
        }

        public MainMenuCubeController()
            :base()
        {
            _currentRotation = Vector3.Zero;
            _period = 2 * new Vector3(3.7f, 3.0f, 5.5f);
            _rotationAmplitude = new Vector3(
                MathHelper.ToRadians(2.0f),
                MathHelper.ToRadians(2.0f),
                MathHelper.ToRadians(2.0f)
                );

            _secondsCount = 0.0f;

            _wobble = true;
            _isAnimating = true;
        }

        public override bool IsAnimating
        {
            get { return _isAnimating; }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_wobble)
            {
                _secondsCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

                _currentRotation = _rotationAmplitude *
                    new Vector3(
                        (float)Math.Sin(MathHelper.TwoPi / _period.X * _secondsCount),
                        (float)Math.Sin(MathHelper.TwoPi / _period.Y * _secondsCount),
                        (float)Math.Sin(MathHelper.TwoPi / _period.Z * _secondsCount)
                     );



                _parentSceneObject.LocalTrans =
                    Matrix.CreateRotationX(_currentRotation.X) *
                    Matrix.CreateRotationY(_currentRotation.Y) *
                    Matrix.CreateRotationZ(_currentRotation.Z);
            }
                

        }
    }
}
