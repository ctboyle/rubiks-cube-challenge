#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagadesCubeWin.Animation;
using RagadesCubeWin.Cameras;
using RagadesCubeWin.GraphicsManagement.BoundingVolumes;

#endregion

namespace RagadesCubeWin.GraphicsManagement
{
    /// <summary>
    /// This serves as the basic object component. It assumes that all Scene Ojbects
    /// will have:
    ///  1. Position in space (world transform)
    ///  2. A bounding Volume
    ///  3. A parent
    ///  4. A position relative to its parent
    /// </summary>
    public abstract class RCSpatial: ISpatial
    {
        protected IRCBoundingVolume _worldBound;

        protected RCSpatial _parentNode;
        protected Matrix _worldTrans;
        private Matrix _localTrans;

        protected List<IController> _animateControllers;


        public RCSpatial parentNode
        {
            get
            {
                return _parentNode;
            }

            set
            {
                _parentNode = value;
            }
        }
        
        public IRCBoundingVolume WorldBound
        {
            get
            {
                return _worldBound;
            }
        }

        public Matrix LocalTrans
        {
            get
            {
                return _localTrans;
            }
            set
            {
                _localTrans = value;
            }

        }

        public Matrix WorldTrans
        {
            get
            {
                return _worldTrans;
            }
        }

        public RCSpatial()
        {
            _localTrans = Matrix.Identity;
            _worldTrans = Matrix.Identity;
            parentNode = null;

            _worldBound = new RCBoundingSphere(
                Vector3.Zero,
                0.0f
                );
                    
            _animateControllers = new List<IController>();

        }

        /// <summary>
        /// Abstract method for loading graphic content.
        /// </summary>
        public abstract void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            );

        /// <summary>
        /// Abstract method for un-loading graphic content.
        /// </summary>
        public abstract void UnloadGraphicsContent();

        /// <summary>
        /// Called to update the SceneObject
        /// 
        /// GS stands for Graphic State
        /// </summary>
        public virtual void UpdateGS(GameTime gameTime, Boolean fInitiator)
        {
            UpdateWorldData(gameTime);
            UpdateWorldBound();
            if (fInitiator)
            {
                PropigateBVToRoot();
            }
        }

        /// <summary>
        /// Override for specific behavior on the draw pass.
        /// </summary>
        public abstract void Draw(GraphicsDevice graphicsDevice);
    


        public bool AttachController(IController controller)
        {
            bool fAttachSucceeded = false;

            if (controller != null)
            {
                _animateControllers.Add(controller);
                fAttachSucceeded = true;
            }

            return fAttachSucceeded;
        }

        public void DetachController(IController controller)
        {
            if (controller != null)
            {
                _animateControllers.Remove(controller);
            }
        }

        protected void UpdateControllers(GameTime gameTime)
        {
            foreach (IController controller in _animateControllers)
            {
                controller.Update(gameTime);
            }
        }

        /// <summary>
        /// Override to update all object world oriented data.
        /// </summary>
        protected virtual void UpdateWorldData(GameTime gameTime)
        {
            // Update animations
            UpdateControllers(gameTime);


            if (parentNode != null)
            {
                // Compute world transform from parent's and local transforms.
                _worldTrans = _localTrans * parentNode.WorldTrans;
            }
            else
            {
                // This is root, local and world trans are identical.
                _worldTrans = _localTrans;
            }
        }
        /// <summary>
        /// Override to specify the world bounding volume for your objects
        /// </summary>
        protected abstract void UpdateWorldBound();

        /// <summary>
        /// If the SceneObject moves and its world BV changes, all its 
        /// parent node's BVs need to be updated.
        ///</summary>
        protected void PropigateBVToRoot()
        {
            if (parentNode != null)
            {
                parentNode.UpdateWorldBound();
                parentNode.PropigateBVToRoot();
            }
        }
    }



}


