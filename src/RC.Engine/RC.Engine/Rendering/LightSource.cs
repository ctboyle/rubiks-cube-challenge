using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.GraphicsManagement;

namespace RC.Engine.Rendering
{
    public class RCLightSource : RCSceneNode
    {
        RCDirectionalLight _light;

        public RCDirectionalLight Light
        {
            get { return _light; }
        }

        public RCLightSource(RCDirectionalLight light)
            : base()
        {
            _light = light;
        }
    }
}
