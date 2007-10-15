using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RagadesCubeWin.Input.Watchers
{
    class Mouse:IWatcher
    {
        #region vars
        List<Input.Events.MouseEvent> lstMouseEvents;
        RealMouseState realstate;
        #endregion

        public Mouse()
        {
            lstMouseEvents = new List<RagadesCubeWin.Input.Events.MouseEvent>();
            realstate = new RealMouseState();
        }

        /// <summary>
        /// Add Event to watcher
        /// </summary>
        /// <param name="e">MouseEvent Type</param>
        /// <returns>True if it adds successfully</returns>
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

        /// <summary>
        /// Add Event to watcher
        /// </summary>
        /// <param name="e">Mouse Event</param>
        /// <returns>True if it adds successfully</returns>
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

        /// <summary>
        /// Remove an event from a watcher to watch for
        /// </summary>
        /// <param name="e">Needs to be KeybaordEvent</param>
        /// <returns>True if successfully removes watched event</returns>
        public bool RemoveEvent(Input.Events.Event e)
        {

            int count = 0;

            try
            {
                Input.Events.MouseEvent me = (Input.Events.MouseEvent)e;

                foreach (Input.Events.MouseEvent ee in lstMouseEvents)
                {
                    if (me.getType()  == ee.getType() && me.getEvent() == ee.getEvent())
                        break;
                    count++;
                }

                if (count == lstMouseEvents.Count)
                {
                    return false;
                }
                else
                {
                    lstMouseEvents.RemoveAt(count);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Detect Mouse
        /// </summary>
        /// <returns>True if it exists</returns>
        public bool DetectMyInput()
        {
            // will assume it exists automatically...
            return true;
        }

        /// <summary>
        /// Run the added events, execute them if true
        /// </summary>
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
