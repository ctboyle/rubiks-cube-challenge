using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.Cameras
{
    class RCCamera : RCNode
    {
        protected Matrix _view;
        protected Matrix _projection;
        protected Viewport _viewport;
        protected float _FOV;

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

        public RCCamera(Viewport newViewport)
            :base()
        {
            SetViewport(newViewport);
            _FOV = MathHelper.PiOver4;
        }

        public void SetViewport(Viewport newViewport)
        {
            _viewport = newViewport;

            _viewport.MinDepth = 1.0f;
			_viewport.MaxDepth = 1000.0f;
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
                _viewport.MinDepth,
                _viewport.MaxDepth
                );

        }
    }
}
