using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace RagadesCubeWin.Input.Watchers
{
    class XBox360GamePad:IWatcher
    {
         #region vars
        List<Input.Events.XBox360GamePadEvent> lstPadEvents;
        RealXBoxGamePad realstate;
        PlayerIndex player;
        #endregion

        public XBox360GamePad(PlayerIndex pi)
        {
            player = pi;
            lstPadEvents = new List<RagadesCubeWin.Input.Events.XBox360GamePadEvent>();
            realstate = new RealXBoxGamePad();
        }

        /// <summary>
        
        /// </summary>
        /// <param name="e"></param>
        /// <returns>True if it adds successfully</returns>
        public bool WatchEvent(Input.Events.Event e)
        {
            try
            {
                lstPadEvents.Add((Input.Events.XBox360GamePadEvent)e);
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
        public bool WatchEvent(Input.Events.XBox360GamePadEvent e)
        {
            try
            {
                lstPadEvents.Add(e);
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
                Input.Events.XBox360GamePadEvent me = (Input.Events.XBox360GamePadEvent)e;

                foreach (Input.Events.XBox360GamePadEvent ee in lstPadEvents)
                {
                    if (me.getType()  == ee.getType() && me.getEvent() == ee.getEvent())
                        break;
                    count++;
                }

                if (count == lstPadEvents.Count)
                {
                    return false;
                }
                else
                {
                    lstPadEvents.RemoveAt(count);
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
            realstate.GamePadState(Microsoft.Xna.Framework.Input.GamePad.GetState(player));

            foreach (Input.Events.XBox360GamePadEvent e in lstPadEvents)
            {
                Input.Events.XBox360GamePadEvent ee = e;

                do
                {
                    if (ee.getEvent() == Input.Types.EventTypes.Pressed)
                    {
                        if (realstate.IsPressed(ee.getType()))
                            ee = ee.execute();
                        else
                            ee = null;
                    }
                    else
                    if (ee.getEvent() == Input.Types.EventTypes.Released)
                    {
                        if(!realstate.IsPressed(ee.getType()))
                            ee = ee.execute();
                        else
                            ee = null;

                    }
                    else
                    if (ee.getEvent() == Input.Types.EventTypes.Tapped)
                    {
                        if(realstate.IsTapped(ee.getType()))
                            ee = ee.execute();
                        else
                            ee = null;

                    }
                    else
                    if (ee.getEvent() == Input.Types.EventTypes.Leaned)
                    {
                        // buttons do not matter for a lean
                        ee = ee.execute(realstate.GetPosition(ee.getType()), realstate.GetHover(ee.getType()));
                    }
                } while (ee != null);
            }
        }
    }
}
