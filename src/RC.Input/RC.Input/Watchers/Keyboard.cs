using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using RC.Input.Events;
using RC.Input.Internal;

namespace RC.Input.Watchers
{
    /// <summary>
    /// Keyboard watcher
    /// Will watch given events, and execute when necessary
    /// </summary>
    public class KeyboardWatcher : RCWatcher<KeyboardEvent>
    {
        #region Vars
        RealKeyboardState realstate;
        #endregion

        /// <summary>
        /// Create new instance of keyboard watcher
        /// </summary>
        public KeyboardWatcher()
        {
            realstate = new RealKeyboardState();
        }

        /// <summary>
        /// Detects input
        /// </summary>
        /// <returns>False if device is not detected</returns>
        public override bool DetectMyInput()
        {
#if XBOX
            return false;
#else
            // will assume it exists automatically...
            return true;
#endif
        }

        /// <summary>
        /// Runs events that matches what is given by the watcher
        /// </summary>
        public override void RunEvents()
        {
            realstate.KeyboardState(Microsoft.Xna.Framework.Input.Keyboard.GetState());

            foreach (Input.Events.KeyboardEvent e in this)
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
