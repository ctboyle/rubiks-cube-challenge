using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RagadesCubeWin.Input.Watchers
{
    class Mouse:IWatcher 
    {
        List<Input.Events.MouseEvent> lstMouseEvents;
        RealMouseState realstate;

        public Mouse()
        {
            lstMouseEvents = new List<RagadesCubeWin.Input.Events.MouseEvent>();
            realstate = new RealMouseState();
        }

        public bool WatchEvent(Input.Events.Event e)
        {
            try
            {
                lstMouseEvents.Add((Input.Events.MouseEvent)e);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WatchEvent(Input.Events.MouseEvent e)
        {
            try
            {
                lstMouseEvents.Add(e);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DetectMyInput()
        {
            // will assume it exists automatically...
            return false;
        }

        public void RunEvents()
        {
            realstate.MouseState(Microsoft.Xna.Framework.Input.Mouse.GetState());

            foreach (Input.Events.MouseEvent e in lstMouseEvents)
            {
                Input.Events.MouseEvent ee = e;

                do
                {
                    if (ee.getEvent() == Input.Types.EventTypes.Pressed)
                    {
                        if (realstate.IsPressed(ee.getType()))
                            ee = ee.execute(realstate.GetPosition(), realstate.GetHover());
                        else
                            ee = null;
                    }
                    else
                        if (ee.getEvent() == Input.Types.EventTypes.Released)
                        {
                            if(!realstate.IsPressed(ee.getType()))
                                ee = ee.execute(realstate.GetPosition(), realstate.GetHover());
                            else
                                ee = null;

                        }
                        else
                            if (ee.getEvent() == Input.Types.EventTypes.Tapped)
                            {
                                if(!realstate.IsTapped(ee.getType()))
                                    ee = ee.execute(realstate.GetPosition(), realstate.GetHover());
                                else
                                    ee = null;

                            }
                            else
                                if (ee.getEvent() == Input.Types.EventTypes.Leaned)
                                {
                                    // buttons do not matter for a lean
                                    ee = ee.execute(realstate.GetPosition(), realstate.GetHover());
                                }
                } while (ee != null);
            }
        }
    }
}
