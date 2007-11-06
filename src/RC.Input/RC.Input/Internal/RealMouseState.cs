using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RC.Input.Internal
{
    internal class RealMouseState
    {


        // Keeps state of button
        struct ButtonState
        {
            public bool OnUp, OnDown, Pressed, WasPressed;
        };

        #region Vars

        ButtonState leftbutton;                 // left button of the mouse
        ButtonState rightbutton;                // right button of the mouse
        Vector2 prevPosition;                   // keeps previous position
        Vector2 curPosition;                    // gets new posisiton
        Vector2 hovered;                        // holds distance traveled

        #endregion

        /// <summary>
        /// Assumes all button states are released
        /// </summary>
        public RealMouseState()
        {
            // just assume everything is unpressed
            leftbutton.Pressed = false;
            leftbutton.OnDown = false;
            leftbutton.WasPressed = false;

            rightbutton.Pressed = false;
            rightbutton.OnDown = false;
            rightbutton.WasPressed = false;
            
        }

        /// <summary>
        /// Tests to see if a particular mouse button is pressed
        /// </summary>
        /// <param name="mbt">Which common mouse button</param>
        /// <returns>True if the button is pressed</returns>
        public bool IsPressed(Input.Types.MouseInput mbt)
        {
            switch (mbt)
            {
                    // left button
                case Input.Types.MouseInput.LeftButton:
                return leftbutton.Pressed;
                    // right button
                case Input.Types.MouseInput.RightButton:
                return rightbutton.Pressed;
                    // unknown button
                default:
                return false;
            };
            
        }

        /// <summary>
        /// Tests to see if a particular mouse button has been clicked
        /// </summary>
        /// <param name="mbt"></param>
        /// <returns></returns>
        public bool IsOnDown(Input.Types.MouseInput mbt)
        {
            switch (mbt)
            {
                    // left button
                case Input.Types.MouseInput.LeftButton:
                    return leftbutton.OnDown;
                    // right button
                case Input.Types.MouseInput.RightButton:
                    return rightbutton.OnDown;
                    // unknown button
                default:
                    return false;
            };
        }

        /// <summary>
        /// Tests to see if a particular mouse button has been clicked
        /// </summary>
        /// <param name="mbt"></param>
        /// <returns></returns>
        public bool IsOnUp(Input.Types.MouseInput mbt)
        {
            switch (mbt)
            {
                // left button
                case Input.Types.MouseInput.LeftButton:
                    return leftbutton.OnUp;
                // right button
                case Input.Types.MouseInput.RightButton:
                    return rightbutton.OnUp;
                // unknown button
                default:
                    return false;
            };
        }

        /// <summary>
        /// Returns the hovered distance
        /// </summary>
        /// <returns>Distance traveled</returns>
        public Vector2 GetHover()
        {
            return hovered;
        }

        /// <summary>
        /// Returns the current position
        /// </summary>
        /// <returns>Current Position</returns>
        public Vector2 GetPosition()
        {
            return curPosition;
        }

        /// <summary>
        /// Take the current mousestate to update the realstate
        /// </summary>
        /// <param name="ms">Current mouse state</param>
        public void MouseState(MouseState ms)
        {
            // update the buttons and hover distance
            #region LeftButton
            // left button
            if (ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                leftbutton.Pressed = true;

                if (leftbutton.WasPressed)
                {
                    leftbutton.WasPressed = true;
                    leftbutton.OnDown = false;

                }
                else
                {
                    leftbutton.WasPressed = true;
                    leftbutton.OnDown = true;
                }
            }
            else if (ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                if (leftbutton.WasPressed)
                {
                    leftbutton.OnUp = true;
                }
                else
                {
                    leftbutton.OnUp = false;
                }
                leftbutton.Pressed = false;
                leftbutton.OnDown = false;
                leftbutton.WasPressed = false;
            }
           
            #endregion

            #region RightButton
            // right button
            if (ms.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                rightbutton.Pressed = true;

                if (rightbutton.WasPressed)
                {
                    rightbutton.WasPressed = true;
                    rightbutton.OnDown = false;
                }
                else
                {
                    rightbutton.WasPressed = true;
                    rightbutton.OnDown = true;
                }
            }
            else
            {
                rightbutton.Pressed = false;
                rightbutton.OnDown = false;
                rightbutton.WasPressed = false;
            }
            #endregion

            #region Hovered,Dragged

            prevPosition = curPosition;
            curPosition = new Vector2((float)ms.X, (float)ms.Y); 
            hovered = new Vector2(prevPosition.X - curPosition.X,
                                    prevPosition.Y - curPosition.Y);
               
       
            #endregion
        }
    }
}
