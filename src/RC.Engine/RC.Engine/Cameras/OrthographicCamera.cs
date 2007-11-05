using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.Cameras
{
    public class RCOrthographicCamera : RCCamera
    {
        private float _width;
        private float _height;

        public float Width 
        {
            get { return _width; }
            set { _width = value; }
        }
        
        public float Height
        {
            get { return _height; }
            set { _height = value;}
        }
        
        public RCOrthographicCamera(Viewport newViewport)
            : base(newViewport)
        {
            _width = 1.0f;
            _height = 1.0f;
        }

        protected override Matrix UpdateProjection()
        {
            return Matrix.CreateOrthographic(
                _width,
                _height,
                _near,
                _far
                );
        }



    }
}
