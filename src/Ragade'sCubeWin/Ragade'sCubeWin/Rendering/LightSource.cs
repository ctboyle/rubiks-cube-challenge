using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GraphicsManagement;

namespace RagadesCubeWin.Rendering
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
