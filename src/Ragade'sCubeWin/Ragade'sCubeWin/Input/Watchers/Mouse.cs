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
                if (e.getEvent() == Input.Types.EventTypes.Pressed)
                {
                    if (realstate.IsPressed(RagadesCubeWin.Input.Types.MouseButtonTypes.LeftButton))
                        e.execute(realstate.GetPosition(), realstate.GetLeftDrag());
                    else if (realstate.IsPressed(RagadesCubeWin.Input.Types.MouseButtonTypes.RightButton))
                        e.execute(realstate.GetPosition(), realstate.GetRightDrag());

                    continue;
                }
                
                if(e.getEvent() == Input.Types.EventTypes.Released)
                {
                    if (!realstate.IsPressed(RagadesCubeWin.Input.Types.MouseButtonTypes.LeftButton))
                        e.execute(realstate.GetPosition(), realstate.GetHover());
                    else if (!realstate.IsPressed(RagadesCubeWin.Input.Types.MouseButtonTypes.RightButton))
                        e.execute(realstate.GetPosition(), realstate.GetHover());
                    continue;
                }

                if (e.getEvent() == Input.Types.EventTypes.Tapped)
                {
                    if (realstate.IsTapped(RagadesCubeWin.Input.Types.MouseButtonTypes.LeftButton))
                        e.execute(realstate.GetPosition(), new Vector2(0,0));
                    else if (realstate.IsTapped(RagadesCubeWin.Input.Types.MouseButtonTypes.RightButton))
                        e.execute(realstate.GetPosition(), new Vector2(0,0));


                    continue;
                }

                // will not look for a click, just movement
                if (e.getEvent() == Input.Types.EventTypes.Leaned)
                {

                    if (!realstate.IsPressed(RagadesCubeWin.Input.Types.MouseButtonTypes.LeftButton) &&
                        !realstate.IsPressed(RagadesCubeWin.Input.Types.MouseButtonTypes.RightButton))
                    {
                        e.execute(realstate.GetPosition(), realstate.GetHover());
                    }
                }
            }
        }
    }
}
