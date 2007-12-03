using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RC.Engine.Animation;
using RC.Engine.GraphicsManagement;
using RagadesCube.SceneObjects;
using RC.Engine.SoundManagement;


namespace RagadesCube.Controllers
{
    public class RCCubeController : Controller<RCCube>
    {
        public event EventHandler OnComplete;

        private List<RCCublet> _cubletFace;
        private float _curRotation;
        private float _radsPerSecond;

        private Vector3 _faceAxis;
        private Matrix _rotMatrix;
        
        public RCCubeController()
            : base()
        {
            _cubletFace = null;
            _isAnimating = false;
            
            // Default speed of rotations are one 1/4 turn per second;
            _radsPerSecond = MathHelper.PiOver2 / 0.25f;
            _rotMatrix = Matrix.Identity;
        }

        public void RotateFace(
            RCCube.FaceSide face, 
            RCCube.RotationDirection direction
            )
        {

            SoundManager.PlayCue("slice");

            RCCube parentCube = (RCCube)_controlledItem;
            List<RCCublet> listCublets = parentCube.GetCubletsOnFace(
                face
                );

            Vector3 faceAxis = parentCube.GetFaceNormal(face);

            if (direction == RCCube.RotationDirection.Clockwise)
            {
                faceAxis *= -1;
            }

            StartAnimation(
                listCublets,
                faceAxis
                );
        }

        public void StartAnimation(List<RCCublet> listCubletsFace, Vector3 faceAxis)
        {
            if (!_isAnimating)
            {

                if (listCubletsFace != null)
                {
                    _cubletFace = listCubletsFace;
                    _curRotation = 0.0f;
                    _isAnimating = true;

                    _faceAxis = faceAxis;
                    _faceAxis.Normalize();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_isAnimating)
            {
                // Update rotation degrees.
                float radians = _radsPerSecond * (float)gameTime.ElapsedRealTime.TotalSeconds;

                _curRotation += radians;

                if (_curRotation > MathHelper.PiOver2)
                {
                    // Dont go over ninety degrees.
   
                    float radOff = _curRotation - MathHelper.PiOver2;
                    radians -= radOff;
                }

                // Form an iterative rotation matrix
                _rotMatrix = Matrix.CreateFromAxisAngle(
                    _faceAxis, 
                    radians
                    );

                foreach (RCCublet cubelet in _cubletFace)
                {
                    cubelet.LocalTrans *= _rotMatrix;
                }

                if (_curRotation >= MathHelper.PiOver2)
                {
                    _isAnimating = false;
                    if (OnComplete != null)
                    {
                        OnComplete(this, null);
                    }
                }
                
            }
            else
            {
                // Do nothing
            }
        }
    }
}
