using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input.Watchers
{
    /// <summary>
    /// Keyboard watcher
    /// Will watch given events, and execute when necessary
    /// </summary>
    public class Keyboard:IWatcher
    {
        #region Vars
        List<Input.Events.KeyboardEvent> lstKeyboardEvents;
        RealKeyboardState realstate;
        #endregion

        /// <summary>
        /// Create new instance of keyboard watcher
        /// </summary>
        public Keyboard()
        {
            lstKeyboardEvents = new List<RagadesCubeWin.Input.Events.KeyboardEvent>();
            realstate = new RealKeyboardState();
        }

        /// <summary>
        /// Add event to watch for
        /// </summary>
        /// <param name="e">KeyboardEvent Type</param>
        /// <returns>True if successfully adds into list</returns>
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
               Input.Events.KeyboardEvent ke = (Input.Events.KeyboardEvent)e;

               foreach(Input.Events.KeyboardEvent ee in lstKeyboardEvents)
               {
                   if (ke.getKey() == ee.getKey() && ke.getEvent() == ee.getEvent())
                       break;
                   count++;
               }

               if (count == lstKeyboardEvents.Count)
               {
                   return false;
               }
               else
               {
                   lstKeyboardEvents.RemoveAt(count);
                   return true;
               }
            }
            catch
            {
                return false;
            }
        }

        ///// <summary>
        ///// Add event to watch for
        ///// </summary>
        ///// <param name="e">KeyboardEvent Type</param>
        ///// <returns>True if successfully adds into list</returns>
        //public bool WatchEvent(Input.Events.KeyboardEvent e)
        //{
        //    try
        //    {
        //        lstKeyboardEvents.Add(e);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Detects input
        /// </summary>
        /// <returns>False if device is not detected</returns>
        public bool DetectMyInput()
        {
            // will assume it exists automatically...
            return true;
        }

        /// <summary>
        /// Runs events that matches what is given by the watcher
        /// </summary>
        public void RunEvents()
        {
            realstate.KeyboardState(Microsoft.Xna.Framework.Input.Keyboard.GetState());

            foreach (Input.Events.KeyboardEvent e in lstKeyboardEvents)
            {
                if (!e.ALL)
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
                        else if (ee.getEventType() == Input.Types.EventTypes.OnDown)
                        {
                            if (realstate.IsOnDown(ee.getKey()))
                                ee = ee.execute();
                            else ee = null;
                        }
                        else if (ee.getEventType() == Input.Types.EventTypes.OnUp)
                        {
                            if (realstate.IsOnUp(ee.getKey()))
                                ee = ee.execute();
                            else ee = null;
                        }
                    } while (ee != null);
                }
                else // must be a general event
                {
                    if (e.getEventType() == Input.Types.EventTypes.Pressed)
                    {
                        for (int i = 0; i < 256; i++)
                        {
                            if (realstate.IsPressed((Keys)i))
                                e.execute((Keys)i);
                        }
                        
                    }
                    else if (e.getEventType() == Input.Types.EventTypes.Released)
                    {
                        for (int i = 0; i < 256; i++)
                        {
                            if (!realstate.IsPressed((Keys)i))
                                e.execute((Keys)i);
                        }
                    }
                    else if (e.getEventType() == Input.Types.EventTypes.OnDown)
                    {
                        for (int i = 0; i < 256; i++)
                        {
                            if (realstate.IsOnDown((Keys)i))
                                e.execute((Keys)i);
                        }
                    }
                    else if (e.getEventType() == Input.Types.EventTypes.OnUp)
                    {
                        for (int i = 0; i < 256; i++)
                        {
                            if (realstate.IsOnUp((Keys)i))
                                e.execute((Keys)i);
                        }
                    }
                }
            }
        }
    }
}
