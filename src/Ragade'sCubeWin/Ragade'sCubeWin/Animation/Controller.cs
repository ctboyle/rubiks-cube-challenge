using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCubeWin.GraphicsManagement;


namespace RagadesCubeWin.Animation
{
    public interface IController
    {
        bool IsAnimating { get; }
        void Update(GameTime gameTime);

        RCSpatial Parent { get;}

    }


    public abstract class Controller<CntrlType> : IController where CntrlType : RCSpatial
    {
        protected CntrlType _parentSceneObject;
        
        public RCSpatial Parent
        {
            get { return _parentSceneObject; }
        }

        public bool AttachToObject(CntrlType parent)
        {
            bool fSuccess = false;
            if (parent != null)
            {
                _parentSceneObject = parent;
                fSuccess = _parentSceneObject.AttachController(this);
            }

            return fSuccess;
        }


        public Controller()
        {
            _parentSceneObject = null;
        }

        public abstract bool IsAnimating { get; }
        public abstract void Update(GameTime gameTime);
    }
}
