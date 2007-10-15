using System;
using System.Collections.Generic;
using System.Text;

namespace RagadesCubeWin.Input
{
    public class Scheme <T> where T:new()
    {
        T controllerItem;
        InputManager inputM;

        Scheme(InputManager im)
        {
            inputM = im;
        }

        public void Apply(T ControllerItem)
        {
            // not yet implemented
        }

        public void Unapply()
        {
            // not yet implemented
        }

        protected void MapWatcherEvents()
        {
            // not implemented
        }
    }
}
