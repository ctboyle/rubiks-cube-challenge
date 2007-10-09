using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input
{
    /// <summary>
    /// Given a new state of the keyboard,
    /// it produce better information about the state itself
    /// </summary>
    class RealKeyboardState
    {
        /// <summary>
        /// used to keep track of state of each button
        /// </summary>
        struct ButtonState
        {
            public bool Tapped, Pressed, WasPressed;
        };

        /// <summary>
        /// Map of all buttons on the keyboard
        /// saves each of their states
        /// </summary>
        static ButtonState[] r_ButtonState = new ButtonState[256];

        /// <summary>
        /// Checks to see if sent key is pressed
        /// </summary>
        /// <param name="key">The key to look at</param>
        /// <returns>True if the key is pressed</returns>
        public bool IsPressed(Keys key)
        {
            return r_ButtonState[(int)key].Pressed;
        }

        /// <summary>
        /// Checks to see if sent key is tapped
        /// </summary>
        /// <param name="key">What key to look at</param>
        /// <returns>True is the key has been tapped</returns>
        public bool IsTapped(Keys key)
        {
            return r_ButtonState[(int)key].Tapped;
        }

        /// <summary>
        /// Updates the real state of the keyboard
        /// </summary>
        /// <param name="ks">The latest state of the keyboard</param>
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
