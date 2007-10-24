using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.GraphicsManagement;

namespace RagadesCubeWin.Rendering
{
    /// <summary>
    /// Represents a Directional light.
    /// 
    /// Children of this node will we rendered with this light enabled.
    /// </summary>
    public class RCDirectionalLight : RCSceneNode
    {
        protected RCLightSource _lightSource;
        protected RCRenderManager.DirectionalLightIndex _index;
        protected Vector3 _direction;
        protected Vector3 _diffuse;
        protected Vector3 _specular;

        public RCRenderManager.DirectionalLightIndex LightIndex
        {
            get
            {
                return _index;
            }
        }
        
        public Vector3 Direction
        {
            get
            {
                return _lightSource.WorldTrans.Forward;
            }
        }

        public Vector3 Diffuse
        {
            get
            {
                return _diffuse;
            }
            set
            {
                _diffuse = value;
            }
        }

        public Vector3 Specular
        {
            get
            {
                return _specular;
            }
            set
            {
                _specular = value;
            }
        }

        public RCLightSource LightSource
        {
            get { return _lightSource; }
        }
        
        public RCDirectionalLight(RCRenderManager.DirectionalLightIndex index)
        {
            _lightSource = new RCLightSource(this);
            _index = index;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            
            // Enable light before rendering children
            RCRenderManager.EnableDirectionalLight(this);
            
            // Draw children.
            base.Draw(graphicsDevice);

            // disable light before continuing drawing in the scene graph.
            RCRenderManager.DisableDirectionalLight(this);
        }

    }
}
