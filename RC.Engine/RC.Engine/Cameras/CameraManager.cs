using System;
using System.Collections;
using System.Text;

using RC.Engine.Rendering;

namespace RC.Engine.Cameras
{
    public class RCCameraManager
    {
        private static Hashtable cameras = new Hashtable();
        private static RCCamera activeCamera;

        public static RCCamera ActiveCamera { get { return activeCamera; } }

        public static void AddCamera(RCCamera newCamera, string cameraLabel)
        {
            cameras.Add(cameraLabel, newCamera);
        }

        public static void RemoveCamera(string cameraLabel)
        {
            cameras.Remove(cameraLabel);
        }

        public static void SetActiveCamera(string cameraLabel)
        {
            if (cameras.ContainsKey(cameraLabel))
            {
                activeCamera = cameras[cameraLabel] as RCCamera;
            }
        }

        public static RCCamera GetCamera(string cameraLabel)
        {
            RCCamera getCamera = null;
            if (cameras.ContainsKey(cameraLabel))
            {
                getCamera = cameras[cameraLabel] as RCCamera;
            }

            return getCamera;
        }
    }
}
