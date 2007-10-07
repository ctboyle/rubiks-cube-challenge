using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.GraphicsManagement;


namespace RagadesCubeWin.Animation
{
    public abstract class Controller
    {
        protected RCSpatial _parentSceneObject;
        
        public RCSpatial Parent
        {
            get { return _parentSceneObject; }
            set { _parentSceneObject = value; }
        }

        public bool AttachParent(RCSpatial parent)
        {
            bool fSuccess = VerifyParentType(parent);

            if (fSuccess)
            {
                _parentSceneObject = parent;
            }

            return fSuccess;
        }

        protected virtual bool VerifyParentType(RCSpatial parent)
        {
            return parent != null;
        }

        public Controller()
        {
            _parentSceneObject = null;
        }

        public abstract bool IsAnimating { get; }
        public abstract void Update(GameTime gameTime);
    }
}
