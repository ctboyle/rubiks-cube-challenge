#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagadesCubeWin.Animation;
using RagadesCubeWin.Cameras;

#endregion

namespace RagadesCubeWin.SceneManagement
{
    /// <summary>
    /// This serves as the basic object component. It assumes that all Scene Ojbects
    /// will have:
    ///  1. Position in space (world transform)
    ///  2. A bounding Volume
    ///  3. A parent
    ///  4. A position relative to its parent
    /// </summary>
    public abstract class RCSceneObject
    {
        protected BoundingSphere _worldBound;
        protected RCNode _parentNode;
        protected Matrix _worldTrans;
        protected Matrix _localTrans;

        protected List<Controller> _animateControllers;


        public RCNode parentNode
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
        
        public BoundingSphere worldBound
        {
            get
            {
                return _worldBound;
            }
            set
            {
                _worldBound = value;
            }
        }

        public Matrix localTrans
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

        public Matrix worldTrans
        {
            get
            {
                return _worldTrans;
            }

            set
            {
                _worldTrans = value;
            }
        }

        public RCSceneObject()
        {
            localTrans = Matrix.Identity;
            worldTrans = Matrix.Identity;
            parentNode = null;
            
            _worldBound.Center = Vector3.Zero;
            _worldBound.Radius = 0.0f;
            _animateControllers = new List<Controller>();

        }

        /// <summary>
        /// Abstract method for loading graphic content.
        /// </summary>
        public abstract void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            );

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
    

#region SceneObject operations

        public void AttachController(Controller controller)
        {
            if (controller != null)
            {
                // May need to use functions instead of property to set parent.
                controller.Parent = this;

                _animateControllers.Add(controller);
            }
        }

        protected void UpdateControllers(GameTime gameTime)
        {
            foreach (Controller controller in _animateControllers)
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
                worldTrans = localTrans * parentNode.worldTrans;
            }
            else
            {
                // This is root, local and world trans are identical.
                worldTrans = localTrans;
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

#endregion
    }



}


