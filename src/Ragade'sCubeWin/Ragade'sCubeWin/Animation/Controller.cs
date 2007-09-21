using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.SceneManagement;


namespace RagadesCubeWin.Animation
{
    public abstract class Controller
    {
        protected RCSceneObject _parentSceneObject;
        
        public RCSceneObject Parent
        {
            get { return _parentSceneObject; }
            set { _parentSceneObject = value; }
        }

        Controller()
        {
            _parentSceneObject = null;
        }

        public abstract void Update(GameTime gameTime);
    }
}
