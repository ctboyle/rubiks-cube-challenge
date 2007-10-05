using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.Cameras
{
    class RCCamera : RCNode
    {
        protected Matrix _view;
        protected Matrix _projection;
        protected Viewport _viewport;
        protected Color _clearColor;
        protected float _FOV;
        protected float _near;
        protected float _far;

        public Matrix View
        {
            get
            {
                return _view;
            }
        }

        public Matrix Projection
        {
            get
            {
                return _projection;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return _viewport;
            }
            set
            {
                _viewport = value;
                UpdateWorldData(null);
            }
        }

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

        public float Near
        {
            get
            {
                return _near;
            }
            set
            {
                _near = value;
            }
        }

        public float Far
        {
            get
            {
                return _far;
            }
            set
            {
                _far = value;
            }
        }

        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
        }

        public RCCamera(Viewport newViewport)
            :base()
        {
            SetViewport(newViewport);
            _FOV = MathHelper.PiOver4;
            _near = 1.0f;
            _far = 1000.0f;

            ClearColor = Color.CornflowerBlue;
        }

        public void SetViewport(Viewport newViewport)
        {
            _viewport = newViewport;

            _viewport.MinDepth = 0.0f;
			_viewport.MaxDepth = 1.0f;
        }


        protected override void UpdateWorldData(GameTime gameTime)
        {
            base.UpdateWorldData(gameTime);

            // Create view matrix from world tranform.
            _view = Matrix.Invert(worldTrans);

            // Create perpective projection Matrix based on viewport
            _projection = Matrix.CreatePerspectiveFieldOfView(
                _FOV,
                (float)_viewport.Width / (float)_viewport.Height,
                _near,
                _far
                );

        }
    }
}
