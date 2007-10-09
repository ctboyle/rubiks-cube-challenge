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
        /// Add event to watch for
        /// </summary>
        /// <param name="e">KeyboardEvent Type</param>
        /// <returns>True if successfully adds into list</returns>
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
