using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input.Watchers
{
    public class Keyboard:IWatcher
    {
        List<Input.Events.KeyboardEvent> lstKeyboardEvents;
        RealKeyboardState realstate;

        public Keyboard()
        {
            lstKeyboardEvents = new List<RagadesCubeWin.Input.Events.KeyboardEvent>();
            realstate = new RealKeyboardState();
        }

        public bool WatchEvent(Input.Events.Event e)
        {
            try
            {
                lstKeyboardEvents.Add((Input.Events.KeyboardEvent)e);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WatchEvent(Input.Events.KeyboardEvent e)
        {
            try
            {
                lstKeyboardEvents.Add(e);
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
            realstate.KeyboardState(Microsoft.Xna.Framework.Input.Keyboard.GetState());

            foreach (Input.Events.KeyboardEvent e in lstKeyboardEvents)
            {
                Input.Events.KeyboardEvent ee = e;
                do
                {

                    if (ee.getEventType() == Input.Types.EventTypes.Pressed)
                    {
                        if (realstate.IsPressed(ee.getKey()))
                            ee = ee.execute();
                        else ee = null;
                    }
                    else if (ee.getEventType() == Input.Types.EventTypes.Released)
                    {
                        if (!realstate.IsPressed(ee.getKey()))
                            ee = ee.execute();
                        else ee = null;
                    }
                    else if (ee.getEventType() == Input.Types.EventTypes.Tapped)
                    {
                        if (realstate.IsTapped(ee.getKey()))
                            ee = ee.execute();
                        else ee = null;
                    }
                } while (ee != null);
            }
        }
    }
}
