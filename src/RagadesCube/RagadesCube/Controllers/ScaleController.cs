using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RC.Engine.GraphicsManagement;
using RC.Engine.Animation;



namespace RagadesCube.Controllers
{
    class ScaleController 
        : RCKeyFrameController<RCSpatial>
    {

        public ScaleController()
            :base()
        {
            DoRotation = false;
            DoTranslation = false;

            ScaleMode = InterpolationMode.SmoothStep;
        }

        public void BeginAnimation(
            Vector3 sourceScale,
            Vector3 destinationScale,
            float duration
            )
        {
            if (!_isAnimating)
            {
                Begin(duration);

               
                _sourceScale = sourceScale;
                _destScale = destinationScale;
            }
        }
    }
}
