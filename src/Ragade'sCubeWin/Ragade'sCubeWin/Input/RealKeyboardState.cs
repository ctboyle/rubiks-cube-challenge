using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input
{
    
    class RealKeyboardState
    {
        struct ButtonState
        {
            public bool Tapped, Pressed, WasPressed;
        };

        static ButtonState[] r_ButtonState = new ButtonState[256];

        public static bool IsPressed(Keys key)
        {
            return r_ButtonState[(int)key].Pressed;
        }

        public static bool IsTapped(Keys key)
        {
            return r_ButtonState[(int)key].Tapped;
        }

        public List<Input.Events.KeyboardEvent> eventList()
        {
            List<Input.Events.KeyboardEvent> el = new List<RagadesCubeWin.Input.Events.KeyboardEvent>();

            for (int i = 0; i < 256; i++)
            {
                if (IsPressed((Keys)i))
                {
                    el.Add(new Input.Events.KeyboardEvent((Keys)i, Input.Types.EventTypes.Pressed,null));
                }
                else  // RELEASED
                {
                    el.Add(new Input.Events.KeyboardEvent((Keys)i, Input.Types.EventTypes.Tapped,null));
                }

                if (IsTapped((Keys)i))
                {
                    el.Add(new Input.Events.KeyboardEvent((Keys)i, Input.Types.EventTypes.Tapped,null));
                }
            }

            return el;
        }

        public void KeyboardState(KeyboardState ks)
        {
            for (int i = 0; i < 256; i++)
                r_ButtonState[i].Pressed = false;

            foreach (Keys m_key in ks.GetPressedKeys())
                r_ButtonState[(int)m_key].Pressed = true;


            for (int i = 0; i < 256; i++)
            {
                if (r_ButtonState[i].Pressed)
                {
                    if (r_ButtonState[i].WasPressed)
                        r_ButtonState[i].Tapped = false;
                    else
                    {
                        r_ButtonState[i].WasPressed = true;
                        r_ButtonState[i].Tapped = true;
                    }

                }
                else
                {
                    r_ButtonState[i].WasPressed = false;
                    r_ButtonState[i].Tapped = false;
                }
            }
        }
    }
}
