using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.SceneManagement;
using Microsoft.Xna.Framework;
using RC.Engine.GraphicsManagement;
using RC.Engine.Cameras;

namespace RC.Engine.Picking
{
    class RCScenePicker
    {
        /// <summary>
        /// Gets the collection of items in the specified scene
        /// that were picked by 2d screen coordinates.
        /// </summary>
        /// <param name="mousePickCoords"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public RCPickRecord Pick(
            Point worldScreenCoords, 
            RCScene scene
            )
        {
            RCCamera camera = RCCameraManager.GetCamera(
                scene.SceneCameraLabel
                );

            if (camera != null)
            {
                // First check to see if the point is in the camera's
                // viewport.
                if (camera.ContainsPoint(worldScreenCoords))
                {
                    // Get the unprojected ray.
                    Ray? worldRay = camera.UnprojectWorldRay(
                        worldScreenCoords
                        );

                    if (worldRay != null)
                    {
                        // Find any intersectinos.
                        return Pick(worldRay.Value, scene.SceneRoot);
                    }
                }
            }

            return new RCPickRecord();
        }


        public RCPickRecord Pick(Ray worldRay, ISpatial sceneRoot)
        {
            RCPickRecord pickRecord = new RCPickRecord();

            PickRecursive(
                worldRay,
                sceneRoot,
                pickRecord
                );

            return pickRecord;
        }

        private void PickRecursive(
            Ray worldRay,
            ISpatial sceneRoot,
            RCPickRecord pickRecord
            )
        {
            // Check for ray intersection with object.
            float? collisionDist = sceneRoot.WorldBound.Intersects(worldRay);

            if (collisionDist != null)
            {
                // Collision, add to found list.
                pickRecord.AddPicked(
                    sceneRoot,
                    collisionDist.Value
                    );
                
                // If object has children, test them.
                if (sceneRoot is INode)
                {
                    List<ISpatial> children =
                        ((INode)sceneRoot).GetChildren();
                    
                    // Pick each of the children
                    foreach (ISpatial child in children)
                    {
                        PickRecursive(
                            worldRay,
                            child,
                            pickRecord
                            );
                    }
                }
            }
        }
    }
}
