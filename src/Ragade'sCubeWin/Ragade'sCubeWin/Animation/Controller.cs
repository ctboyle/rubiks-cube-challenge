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

        public bool AttachParent(RCSceneObject parent)
        {
            bool fSuccess = VerifyParentType(parent);

            if (fSuccess)
            {
                _parentSceneObject = parent;
            }

            return fSuccess;
        }

        protected virtual bool VerifyParentType(RCSceneObject parent)
        {
            return parent != null;
        }

        public Controller()
        {
            _parentSceneObject = null;
        }

        public abstract void Update(GameTime gameTime);
    }
}
