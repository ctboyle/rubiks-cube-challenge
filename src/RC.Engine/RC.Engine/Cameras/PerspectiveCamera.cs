using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RC.Engine.GraphicsManagement;
using RC.Engine.Rendering;

namespace RC.Engine.Cameras
{
    public class RCPerspectiveCamera : RCCamera
    {
        protected float _FOV;


        // Field of View property
        public float FOV
        {
            get
            {
                return _FOV;
            }
            set
            {
                _FOV = value;
                UpdateWorldData(null);
            }
        }


        public RCPerspectiveCamera(Viewport newViewport)
            : base(newViewport)
        {
            _FOV = MathHelper.PiOver4;
            
        }

        protected override Matrix UpdateProjection()
        {
            // Create perpective projection Matrix based on viewport
            return Matrix.CreatePerspectiveFieldOfView(
               _FOV,
               (float)_viewport.Width / (float)_viewport.Height,
               _near,
               _far
               );
        }
    }
}
