using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.GraphicsManagement;

namespace RC.Engine.SceneManagement
{
    public interface IRCSceneCreator
    {
        RCScene CreateScene();
    }
}
