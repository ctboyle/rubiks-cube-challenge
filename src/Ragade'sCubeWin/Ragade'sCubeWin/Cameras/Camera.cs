using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.Cameras
{
    public abstract class RCCamera : RCSceneNode
    {
        protected Matrix _view;
        protected Matrix _projection;
        protected Viewport _viewport;
        protected Color _clearColor;

        protected ClearOptions _clearOptions;

        protected float _near;
        protected float _far;
        
        bool _clearScreen;

        public ClearOptions ClearOptions
        {
            get { return _clearOptions; }
            set { _clearOptions = value; }
        }

        public bool ClearScreen
        {
            get { return _clearScreen; }
            set { _clearScreen = value; }
        }
         
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

            _clearScreen = true;
            _clearOptions = 
                ClearOptions.DepthBuffer |
                ClearOptions.Target;

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

            _view = UpdateView();

            _projection = UpdateProjection();

        }

        private Matrix UpdateView()
        {
            // Create view matrix from world tranform.
            return Matrix.Invert(WorldTrans);
        }

        /// <summary>
        /// Determins the projection matrix for the camera.
        /// </summary>
        /// <returns></returns>
        protected abstract Matrix UpdateProjection();

    }
}
