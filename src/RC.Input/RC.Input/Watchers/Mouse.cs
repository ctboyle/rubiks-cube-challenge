using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using RC.Input.Events;
using RC.Input.Internal;

namespace RC.Input.Watchers
{
    class MouseWatcher : RCWatcher<MouseEvent>
    {
        #region vars
        RealMouseState realstate;
        #endregion

        public MouseWatcher()
        {
            realstate = new RealMouseState();
        }

        /// <summary>
        /// Detect Mouse
        /// </summary>
        /// <returns>True if it exists</returns>
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
        /// Run the added events, execute them if true
        /// </summary>
        public override void RunEvents()
        {
            realstate.MouseState(Microsoft.Xna.Framework.Input.Mouse.GetState());

            foreach (Input.Events.MouseEvent e in this)
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
                    if (ee.getEvent() == Input.Types.EventTypes.OnDown)
                    {
                        if(realstate.IsOnDown(ee.getType()))
                            ee = ee.execute(realstate.GetPosition(), realstate.GetHover());
                        else
                            ee = null;

                    }
                    else
                    if (ee.getEvent() == Input.Types.EventTypes.OnUp)
                    {
                        if (realstate.IsOnUp(ee.getType()))
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
