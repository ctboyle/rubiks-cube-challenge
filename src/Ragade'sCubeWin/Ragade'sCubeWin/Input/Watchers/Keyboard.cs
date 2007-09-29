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
                if (e.getEvent() == Input.Types.EventTypes.Pressed)
                {
                    if(realstate.IsPressed(e.getKey()))
                        e.execute();

                    continue;
                }
                
                if(e.getEvent() == Input.Types.EventTypes.Released)
                {
                    if (!realstate.IsPressed(e.getKey()))
                        e.execute();

                    continue;
                }

                if (e.getEvent() == Input.Types.EventTypes.Tapped)
                {
                    if (realstate.IsTapped(e.getKey()))
                        e.execute();

                    continue;
                }
            }
        }
    }
}
