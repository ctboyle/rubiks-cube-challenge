#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace RagadesCubeWin.SceneManagement
{
    /// <summary>
    /// Implements the ablity for a SceneObject to have Children.
    /// 
    /// The RCNode's bounding volume is computed by merging all of its children.
    /// </summary>
    public class RCNode : RCSceneObject
    {
        protected List<RCSceneObject> listChildren;
        
        public RCNode()
        {
            listChildren = new List<RCSceneObject>();
        }

        /// <summary>
        /// Adds a SceneObject to Children.
        /// </summary>
        public void AddChild(RCSceneObject newChild)
        {
            newChild.parentNode = this;
            listChildren.Add(newChild);
        }


        /// <summary>
        /// Invokes all children's LoadGraphicsContent.
        /// </summary>
        public override void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            )
        {
            foreach (RCSceneObject child in listChildren)
            {
                child.LoadGraphicsContent(
                    graphics,
                    content
                );

            }
        }

        /// <summary>
        /// Draws all children 
        /// </summary>
        public override void Draw(GraphicsDevice graphicsDevice)
        {           
            foreach (RCSceneObject child in listChildren)
            {
                child.Draw(graphicsDevice);
            }
        }

        /// <summary>
        /// Overriden. Updates the  node's world data.
        /// 
        /// Calls all the chilren's Update methods.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void UpdateWorldData(GameTime gameTime)
        {
            base.UpdateWorldData(gameTime);

            foreach (RCSceneObject child in listChildren)
            {
                child.UpdateGS(gameTime, true);
            }
        }

        /// <summary>
        /// Overriden. Will ensure the node's BV is the correct size.
        /// 
        /// Calls all the chilren's Update methods.
        /// </summary>
        protected override void UpdateWorldBound()
        {
            // Update the Node's bounding volume size so that it is the
            // smallest volume that can contains all the children BVs.

            Boolean fFirstChild = true;
            foreach (RCSceneObject child in listChildren)
            {    
                // Use the first child to define the initial BV.
                if (fFirstChild)
                {
                    // TODO: See if I need to use 'clone()'
                    worldBound = child.worldBound;
                    fFirstChild = false;
                }
                else
                {
                    // Merge the remaining children's BVs.
                    worldBound = BoundingSphere.CreateMerged(
                        worldBound,
                        child.worldBound
                    ); 
                }
            }
        }
    }
}


