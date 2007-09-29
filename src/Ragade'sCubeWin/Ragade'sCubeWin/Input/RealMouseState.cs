using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input
{
    class RealMouseState
    {



        struct ButtonState
        {
            public bool Tapped, Pressed, WasPressed;
        };

        #region Vars

        ButtonState leftbutton;
        ButtonState rightbutton;
        Vector2 prevPosition;
        Vector2 curPosition;
        Vector2 hovered;
        Vector2 ldragged;
        Vector2 rdragged;
        #endregion

        public RealMouseState()
        {
            leftbutton.Pressed = false;
            leftbutton.Tapped = false;
            leftbutton.WasPressed = false;

            rightbutton.Pressed = false;
            rightbutton.Tapped = false;
            rightbutton.WasPressed = false;
            
        }

        public bool IsPressed(Input.Types.MouseButtonTypes mbt)
        {
            switch (mbt)
            {
                case Input.Types.MouseButtonTypes.LeftButton:
                return leftbutton.Pressed;
                
                case Input.Types.MouseButtonTypes.RightButton:
                return rightbutton.Pressed;
                
                default:
                return false;
            };
            
        }

        public bool IsTapped(Input.Types.MouseButtonTypes mbt)
        {
            switch (mbt)
            {
                case Input.Types.MouseButtonTypes.LeftButton:
                    return leftbutton.Tapped;

                case Input.Types.MouseButtonTypes.RightButton:
                    return rightbutton.Tapped;

                default:
                    return false;
            };
        }

        public Vector2 GetHover()
        {
            return hovered;
        }

        public Vector2 GetLeftDrag()
        {
            return ldragged;
        }

        public Vector2 GetRightDrag()
        {
            return rdragged;
        }

        public Vector2 GetPosition()
        {
            return curPosition;
        }

        // add functions (HOvered, dragged, what not)

        public void MouseState(MouseState ms)
        {
            #region LeftButton
            // left button
            if (ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                leftbutton.Pressed = true;

                if (leftbutton.WasPressed)
                {
                    leftbutton.WasPressed = true;
                    leftbutton.Tapped = false;

                }
                else
                {
                    leftbutton.WasPressed = true;
                    leftbutton.Tapped = true;
                }
            }
            else
            {
                leftbutton.Pressed = false;
                leftbutton.Tapped = false;
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
                    rightbutton.Tapped = false;
                }
                else
                {
                    rightbutton.WasPressed = true;
                    rightbutton.Tapped = true;
                }
            }
            else
            {
                rightbutton.Pressed = false;
                rightbutton.Tapped = false;
                rightbutton.WasPressed = false;
            }
            #endregion

            #region Hovered,Dragged

            prevPosition = curPosition;
            curPosition = new Vector2((float)ms.X, (float)ms.Y);

            if (leftbutton.WasPressed)
            {
                hovered = new Vector2(0, 0);
                rdragged = new Vector2(0, 0);

                ldragged = new Vector2(prevPosition.X - curPosition.X,
                                        prevPosition.Y - curPosition.Y);

            }
            else if (rightbutton.WasPressed)
            {
                hovered = new Vector2(0, 0);
                ldragged = new Vector2(0, 0);

                rdragged = new Vector2(prevPosition.X - curPosition.X,
                                        prevPosition.Y - curPosition.Y);
              
            }
            else
            {
                hovered = new Vector2(prevPosition.X - curPosition.X,
                                        prevPosition.Y - curPosition.Y);
                ldragged = new Vector2(0, 0);
                rdragged = new Vector2(0,0);
              
            }
            #endregion
        }
    }
}
