using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;


using RC.Engine.GraphicsManagement;


namespace RC.Engine.Animation
{
    public interface IController
    {
        bool IsAnimating { get; }
        void Update(GameTime gameTime);

        RCSpatial Parent { get;}

    }

  
    public abstract class Controller<CntrlType> : IController where CntrlType : RCSpatial
    {
        protected CntrlType _controlledItem;
        protected bool _isAnimating;
        
       
        public RCSpatial Parent
        {
            get { return _controlledItem; }
        }

        public bool IsAnimating 
        {
            get { return _isAnimating; }
        }

        public bool AttachToObject(CntrlType parent)
        {
            bool fSuccess = false;
            if (parent != null)
            {
                _controlledItem = parent;
                fSuccess = _controlledItem.AddController(this);
            }

           
            return fSuccess;
        }


        public Controller()
        {
            _controlledItem = null;
            _isAnimating = false;
        }

        
        public abstract void Update(GameTime gameTime);
    }
}
