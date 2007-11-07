#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RC.Engine.GraphicsManagement;
using RC.Engine.GraphicsManagement.BoundingVolumes;
using RC.Engine.Rendering;
using RC.Engine.Cameras;
#endregion

namespace RagadesCube.SceneObjects
{
    /// <summary>
    /// This represents the smaller unit cubes on the Rubics Cube.
    /// </summary>
    public class RCCublet : RCSceneNode
    {
        public enum FaceletPosition
        {
            Top,
            Left,
            Back,
            Right,
            Front,
            Bottom,
            Count
        }

        private RCFacelet[] _facelets;
        

        public const float CubeletSize = 2.0f;

        // Space between cubelet and facelet
        private const float FaceletSpacing = 0.01f;

        private RCBoundingSphere _localBound;

        
        public RCFacelet[] Facelets
        {
            get
            {
                return _facelets;
            }
        }



        public RCCublet(int xCubePos, int yCubePos, int zCubePos)
            : base()
        {
            _facelets = new RCFacelet[(int)FaceletPosition.Count];

            BuildLocalBound();

            BuildLocalTransform(xCubePos, yCubePos, zCubePos);
            AddChild(new RCCubeletBox());
        }

        private void BuildLocalBound()
        {
            // Create bounding volume the size of the cubelets

            float halfSize = CubeletSize / 2.0f;
            float radius = halfSize * (float)(Math.Sqrt(2.0)/2.0);

            _localBound = new RCBoundingSphere(
                Vector3.Zero,
                radius
                );
        }

        private void BuildLocalTransform(
            int xCubePos,
            int yCubePos,
            int zCubePos
            )
        {
            LocalTrans = Matrix.CreateTranslation(
                xCubePos * CubeletSize,
                yCubePos * CubeletSize,
                zCubePos * CubeletSize
            );
        }
        
        public void AttachFacelet(FaceletPosition position, RCFacelet facelet)
        {
            if (facelet != null)
            {
                _facelets[(int)position] = facelet;
                BuildFaceletTransform(position, facelet);
                AddChild(facelet);
            }
            else
            {
                throw new ArgumentNullException("facelet", "Passed in null facelet.") ;
            }
        }

        private void BuildFaceletTransform(FaceletPosition position, RCFacelet facelet)
        {
            // The facelet starts out in the center facing towards positive Z.
            Matrix localTransform = Matrix.Identity;
            
            // We move the facelet forward
            Vector3 translate = new Vector3(0, 0, CubeletSize/2.0f + FaceletSpacing);


            // Now we rotate into the desired facelet position
            float xRot = 0.0f;
            float yRot = 0.0f;
            
            switch (position)
            {
                case FaceletPosition.Top:
                    xRot = -MathHelper.PiOver2;
                    break;
                case FaceletPosition.Bottom:
                    xRot = MathHelper.PiOver2;
                    break;
                case FaceletPosition.Left:
                    yRot = -MathHelper.PiOver2;
                    break;
                case FaceletPosition.Right:
                    yRot = MathHelper.PiOver2;
                    break;
                case FaceletPosition.Front:
                    // Do nothing already there
                    break;
                case FaceletPosition.Back:
                    // Rotate 180 degrees
                    yRot = MathHelper.Pi;
                    break;
            }

            // Form transform
            localTransform = Matrix.CreateTranslation(translate) *
                Matrix.CreateRotationX(xRot) * 
                Matrix.CreateRotationY(yRot);

            facelet.LocalTrans = localTransform;
        }

        // Constant sized bounding voulume
        protected override void UpdateWorldBound()
        {
            _worldBound = _localBound.Transform(_worldTrans);
        }
    }
}


