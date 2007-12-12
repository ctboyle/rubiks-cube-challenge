using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RagadesCubeWin.Input.Events;


namespace RagadesCubeWin.Input.Watchers
{
    class XBox360GamePad : RCWatcher<XBox360GamePadEvent>
    { 
         #region vars
        RealXBoxGamePad realstate;
        PlayerIndex player;
        #endregion

        public XBox360GamePad(PlayerIndex pi)
        {
            player = pi;
            realstate = new RealXBoxGamePad();
        }

        /// <summary>
        /// Detect Mouse
        /// </summary>
        /// <returns>True if it exists</returns>
        public override bool DetectMyInput()
        {
            // will assume it exists automatically...
            return Microsoft.Xna.Framework.Input.GamePad.GetState(player).IsConnected;
        }

        /// <summary>
        /// Run the added events, execute them if true
        /// </summary>
        public override void RunEvents()
        {
            realstate.GamePadState(Microsoft.Xna.Framework.Input.GamePad.GetState(player));

            foreach (Input.Events.XBox360GamePadEvent e in this)
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
                    if (ee.getEvent() == Input.Types.EventTypes.OnDown)
                    {
                        if(realstate.IsOnDown(ee.getType()))
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
