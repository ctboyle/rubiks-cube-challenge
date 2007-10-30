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
            public bool OnDown, Pressed, WasPressed;
        };

        /// <summary>
        /// Map of all buttons on the keyboard
        /// saves each of their states
        /// </summary>
        ButtonState[] r_ButtonState = new ButtonState[256];

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
        public bool IsOnDown(Keys key)
        {
            return r_ButtonState[(int)key].OnDown;
        }

        /// <summary>
        /// Checks to see if the key was released
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if the key was pressed, but no longer pressed</returns>
        public bool IsOnUp(Keys key)
        {
            if(r_ButtonState[(int)key].WasPressed && !r_ButtonState[(int)key].Pressed)
                return true;
            return false;
        }

        /// <summary>
        /// Updates the real state of the keyboard
        /// </summary>
        /// <param name="ks">The latest state of the keyboard</param>
        public void KeyboardState(KeyboardState ks)
        {
            for (int i = 0; i < 256; i++)
            {
                if (r_ButtonState[i].WasPressed && !r_ButtonState[i].Pressed)
                    r_ButtonState[i].WasPressed = false;
            }

            for (int i = 0; i < 256; i++)
                r_ButtonState[i].Pressed = false;

            foreach (Keys m_key in ks.GetPressedKeys())
                r_ButtonState[(int)m_key].Pressed = true;


            for (int i = 0; i < 256; i++)
            {
                if (r_ButtonState[i].Pressed)
                {
                    if (r_ButtonState[i].WasPressed)
                        r_ButtonState[i].OnDown = false;
                    else
                    {
                        r_ButtonState[i].WasPressed = true;
                        r_ButtonState[i].OnDown = true;
                    }

                }
                else
                {
                    // for keyup, we need to know it was pressed
                    //r_ButtonState[i].WasPressed = false;
                    r_ButtonState[i].OnDown = false;
                }
            }
        }
    }
}
