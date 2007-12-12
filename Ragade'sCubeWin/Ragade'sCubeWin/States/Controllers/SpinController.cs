using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Animation;


namespace RagadesCubeWin.States.Controllers
{
    class SpinController : Controller<RCSpatial>
    {
        Vector3 _axis;
        float _angularVelocity;

        public Vector3 Axis
        {
            get { return _axis; }
            set { _axis = value; }
        }

        public float AngularVelocity
        {
            get { return _angularVelocity; }
            set { _angularVelocity = value; }
        }

        public SpinController()
            :base()
        {
        }

        public SpinController(
            Vector3 axis,
            float angularVelocity
            )
            : base()
        {
            _axis = axis;
            _angularVelocity = angularVelocity;
        }

        public bool Enabled
        {
            get { return _isAnimating; }
            set { _isAnimating = value; }
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                float angle = (float)(_angularVelocity * gameTime.ElapsedGameTime.TotalSeconds);

                _controlledItem.LocalTrans *=
                    Matrix.CreateFromAxisAngle(
                        _axis,
                        angle
                        );

            }
        }



    }
}
