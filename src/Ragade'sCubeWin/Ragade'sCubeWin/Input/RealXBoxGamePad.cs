using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input
{
    class RealXBoxGamePad
    {
        // Keeps state of button
        struct ButtonState
        {
            public bool Tapped, Pressed, WasPressed;
        };

        ButtonState Y;
        ButtonState X;
        ButtonState A;
        ButtonState B;
        ButtonState START;
        ButtonState BACK;
        ButtonState DUP;
        ButtonState DDOWN;
        ButtonState DLEFT;
        ButtonState DRIGHT;
        ButtonState SHOULDERLEFT;
        ButtonState SHOULDERRIGHT;

        Vector2 LeftAnalogCurrentPosition;
        Vector2 LeftAnalogPrevPosition;
        Vector2 LeftAnalogHover;

        Vector2 RightAnalogCurrentPosition;
        Vector2 RightAnalogPrevPosition;
        Vector2 RightAnalogHover;

        Vector2 RightShoulderCurrentPosition;
        Vector2 RightShoulderPrevPosition;
        Vector2 RightShoulderHover;

        Vector2 LeftShoulderCurrentPosition;
        Vector2 LeftShoulderPrevPosition;
        Vector2 LeftShoulderHover;


        public RealXBoxGamePad()
        {
            #region INITIALIZE THE VALUES

            Y.Pressed = false;
            Y.Tapped = false;
            Y.WasPressed = false;

            X.Pressed = false;
            X.Tapped = false;
            X.WasPressed = false;

            B.Pressed = false;
            B.Tapped = false;
            B.WasPressed = false;

            A.Pressed = false;
            A.Tapped = false;
            A.WasPressed = false;

            START.Pressed = false;
            START.Tapped = false;
            START.WasPressed = false;

            BACK.Pressed = false;
            BACK.Tapped = false;
            BACK.WasPressed = false;
            
            DUP.Pressed = false;
            DUP.Tapped = false;
            DUP.WasPressed = false;

            DDOWN.Pressed = false;
            DDOWN.Tapped = false;
            DDOWN.WasPressed = false;
            
            DLEFT.Pressed = false;
            DLEFT.Tapped = false;
            DLEFT.WasPressed = false;
            
            DRIGHT.Pressed = false;
            DRIGHT.Tapped = false;
            DRIGHT.WasPressed = false;

            SHOULDERLEFT.Pressed = false;
            SHOULDERLEFT.Tapped = false;
            SHOULDERLEFT.WasPressed = false;

            SHOULDERRIGHT.Pressed = false;
            SHOULDERRIGHT.Tapped = false;
            SHOULDERRIGHT.WasPressed = false;
            
            LeftAnalogCurrentPosition = new Vector2(0,0);
            LeftAnalogPrevPosition = new Vector2(0, 0);
            LeftAnalogHover = new Vector2(0, 0);

            RightAnalogCurrentPosition = new Vector2(0, 0);
            RightAnalogPrevPosition = new Vector2(0, 0);
            RightAnalogHover = new Vector2(0, 0);

            RightShoulderCurrentPosition = new Vector2(0, 0);
            RightShoulderPrevPosition = new Vector2(0, 0);
            RightShoulderHover = new Vector2(0, 0);

            LeftShoulderCurrentPosition = new Vector2(0, 0);
            LeftShoulderPrevPosition = new Vector2(0, 0);
            LeftShoulderHover = new Vector2(0, 0);
            #endregion
        }

        /// <summary>
        /// Tests to see if a particular mouse button is pressed
        /// </summary>
        /// <param name="mbt">Which common mouse button</param>
        /// <returns>True if the button is pressed</returns>
        public bool IsPressed(Input.Types.XBox360GamePadTypes gbt)
        {
            switch (gbt)
            {
                // A
                case Input.Types.XBox360GamePadTypes.A:
                    return A.Pressed;
                // B
                case Input.Types.XBox360GamePadTypes.B:
                    return B.Pressed;
                // X
                case Input.Types.XBox360GamePadTypes.X:
                    return X.Pressed;
                // Y
                case Input.Types.XBox360GamePadTypes.Y:
                    return Y.Pressed;
                // START
                case Input.Types.XBox360GamePadTypes.START:
                    return START.Pressed;
                // BACK
                case Input.Types.XBox360GamePadTypes.BACK:
                    return BACK.Pressed;
                // DUP
                case Input.Types.XBox360GamePadTypes.DUP:
                    return DUP.Pressed;
                // DDOWN
                case Input.Types.XBox360GamePadTypes.DDOWN:
                    return DDOWN.Pressed;
                // DRIGHT
                case Input.Types.XBox360GamePadTypes.DRIGHT:
                    return DRIGHT.Pressed;
                // DLEFT
                case Input.Types.XBox360GamePadTypes.DLEFT:
                    return DLEFT.Pressed;
                // RIGHTSHOULDER
                case Input.Types.XBox360GamePadTypes.RIGHTSHOULDER:
                    return SHOULDERRIGHT.Pressed;
                // LEFTSHOULDER
                case Input.Types.XBox360GamePadTypes.LEFTSHOULDER:
                    return SHOULDERLEFT.Pressed;
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
        public bool IsTapped(Input.Types.XBox360GamePadTypes gbt)
        {
            switch (gbt)
            {
                // A
                case Input.Types.XBox360GamePadTypes.A:
                    return A.Tapped;
                // B
                case Input.Types.XBox360GamePadTypes.B:
                    return B.Tapped;
                // X
                case Input.Types.XBox360GamePadTypes.X:
                    return X.Tapped;
                // Y
                case Input.Types.XBox360GamePadTypes.Y:
                    return Y.Tapped;
                // START
                case Input.Types.XBox360GamePadTypes.START:
                    return START.Tapped;
                // BACK
                case Input.Types.XBox360GamePadTypes.BACK:
                    return BACK.Tapped;
                // DUP
                case Input.Types.XBox360GamePadTypes.DUP:
                    return DUP.Tapped;
                // DDOWN
                case Input.Types.XBox360GamePadTypes.DDOWN:
                    return DDOWN.Tapped;
                // DRIGHT
                case Input.Types.XBox360GamePadTypes.DRIGHT:
                    return DRIGHT.Tapped;
                // DLEFT
                case Input.Types.XBox360GamePadTypes.DLEFT:
                    return DLEFT.Tapped;
                // RIGHTSHOULDER
                case Input.Types.XBox360GamePadTypes.RIGHTSHOULDER:
                    return SHOULDERRIGHT.Tapped;
                // LEFTSHOULDER
                case Input.Types.XBox360GamePadTypes.LEFTSHOULDER:
                    return SHOULDERLEFT.Tapped;
                // unknown button
                default:
                    return false;
            };
        }

        /// <summary>
        /// Returns the hovered distance
        /// </summary>
        /// <returns>Distance traveled</returns>
        public Vector2 GetHover(Input.Types.XBox360GamePadTypes gbt)
        {
            switch (gbt)
            {
                case Input.Types.XBox360GamePadTypes.LEFTANALOG:
                    return LeftAnalogHover;
                case Input.Types.XBox360GamePadTypes.RIGHTANALOG:
                    return RightAnalogHover;
                case Input.Types.XBox360GamePadTypes.LEFTTRIGGER:
                    return LeftShoulderHover;
                case Input.Types.XBox360GamePadTypes.RIGHTTRIGGER:
                    return RightShoulderHover;
                default:
                    return new Vector2(0, 0);
            };
           
        }

        /// <summary>
        /// Returns the current position
        /// </summary>
        /// <returns>Current Position</returns>
        public Vector2 GetPosition(Input.Types.XBox360GamePadTypes gbt)
        {
            switch (gbt)
            {
                case Input.Types.XBox360GamePadTypes.LEFTANALOG:
                    return LeftAnalogCurrentPosition;
                case Input.Types.XBox360GamePadTypes.RIGHTANALOG:
                    return RightAnalogCurrentPosition;
                case Input.Types.XBox360GamePadTypes.LEFTTRIGGER:
                    return LeftShoulderCurrentPosition;
                case Input.Types.XBox360GamePadTypes.RIGHTTRIGGER:
                    return RightShoulderCurrentPosition;
                default:
                    return new Vector2(0, 0);
            };
            
        }

        public void GamePadState(GamePadState gs)
        {
            #region A
            if (gs.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                A.Pressed = true;

                if (A.WasPressed)
                {
                    A.WasPressed = true;
                    A.Tapped = false;

                }
                else
                {
                    A.WasPressed = true;
                    A.Tapped = true;
                }
            }
            else
            {
                A.Pressed = false;
                A.Tapped = false;
                A.WasPressed = false;
            }
            #endregion

            #region B
            if (gs.Buttons.B == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                B.Pressed = true;

                if (B.WasPressed)
                {
                    B.WasPressed = true;
                    B.Tapped = false;

                }
                else
                {
                    B.WasPressed = true;
                    B.Tapped = true;
                }
            }
            else
            {
                B.Pressed = false;
                B.Tapped = false;
                B.WasPressed = false;
            }
            #endregion

            #region X
            if (gs.Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                X.Pressed = true;

                if (X.WasPressed)
                {
                    X.WasPressed = true;
                    X.Tapped = false;

                }
                else
                {
                    X.WasPressed = true;
                    X.Tapped = true;
                }
            }
            else
            {
                X.Pressed = false;
                X.Tapped = false;
                X.WasPressed = false;
            }
            #endregion

            #region Y
            if (gs.Buttons.Y == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Y.Pressed = true;

                if (Y.WasPressed)
                {
                    Y.WasPressed = true;
                    Y.Tapped = false;

                }
                else
                {
                    Y.WasPressed = true;
                    Y.Tapped = true;
                }
            }
            else
            {
                Y.Pressed = false;
                Y.Tapped = false;
                Y.WasPressed = false;
            }
            #endregion

            #region START
            if (gs.Buttons.Start == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                START.Pressed = true;

                if (START.WasPressed)
                {
                    START.WasPressed = true;
                    START.Tapped = false;

                }
                else
                {
                    START.WasPressed = true;
                    START.Tapped = true;
                }
            }
            else
            {
                START.Pressed = false;
                START.Tapped = false;
                START.WasPressed = false;
            }
            #endregion

            #region BACK
            if (gs.Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                BACK.Pressed = true;

                if (BACK.WasPressed)
                {
                    BACK.WasPressed = true;
                    BACK.Tapped = false;

                }
                else
                {
                    BACK.WasPressed = true;
                    BACK.Tapped = true;
                }
            }
            else
            {
                BACK.Pressed = false;
                BACK.Tapped = false;
                BACK.WasPressed = false;
            }
            #endregion

            #region DUP
            if (gs.DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                DUP.Pressed = true;

                if (DUP.WasPressed)
                {
                    DUP.WasPressed = true;
                    DUP.Tapped = false;

                }
                else
                {
                    DUP.WasPressed = true;
                    DUP.Tapped = true;
                }
            }
            else
            {
                DUP.Pressed = false;
                DUP.Tapped = false;
                DUP.WasPressed = false;
            }
            #endregion

            #region DDOWN
            if (gs.DPad.Down  == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                DDOWN.Pressed = true;

                if (DDOWN.WasPressed)
                {
                    DDOWN.WasPressed = true;
                    DDOWN.Tapped = false;

                }
                else
                {
                    DDOWN.WasPressed = true;
                    DDOWN.Tapped = true;
                }
            }
            else
            {
                DDOWN.Pressed = false;
                DDOWN.Tapped = false;
                DDOWN.WasPressed = false;
            }
            #endregion

            #region DLEFT
            if (gs.DPad.Left == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                DLEFT.Pressed = true;

                if (DLEFT.WasPressed)
                {
                    DLEFT.WasPressed = true;
                    DLEFT.Tapped = false;

                }
                else
                {
                    DLEFT.WasPressed = true;
                    DLEFT.Tapped = true;
                }
            }
            else
            {
                DLEFT.Pressed = false;
                DLEFT.Tapped = false;
                DLEFT.WasPressed = false;
            }
            #endregion

            #region DRIGHT
            if (gs.DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                DRIGHT.Pressed = true;

                if (DRIGHT.WasPressed)
                {
                    DRIGHT.WasPressed = true;
                    DRIGHT.Tapped = false;

                }
                else
                {
                    DRIGHT.WasPressed = true;
                    DRIGHT.Tapped = true;
                }
            }
            else
            {
                DRIGHT.Pressed = false;
                DRIGHT.Tapped = false;
                DRIGHT.WasPressed = false;
            }
            #endregion

            #region SHOULDERLEFT
            if (gs.Buttons.LeftShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                SHOULDERLEFT.Pressed = true;

                if (SHOULDERLEFT.WasPressed)
                {
                    SHOULDERLEFT.WasPressed = true;
                    SHOULDERLEFT.Tapped = false;

                }
                else
                {
                    SHOULDERLEFT.WasPressed = true;
                    SHOULDERLEFT.Tapped = true;
                }
            }
            else
            {
                SHOULDERLEFT.Pressed = false;
                SHOULDERLEFT.Tapped = false;
                SHOULDERLEFT.WasPressed = false;
            }
            #endregion

            #region SHOULDERRIGHT
            if (gs.Buttons.RightShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                SHOULDERRIGHT.Pressed = true;

                if (SHOULDERRIGHT.WasPressed)
                {
                    SHOULDERRIGHT.WasPressed = true;
                    SHOULDERRIGHT.Tapped = false;

                }
                else
                {
                    SHOULDERRIGHT.WasPressed = true;
                    SHOULDERRIGHT.Tapped = true;
                }
            }
            else
            {
                SHOULDERRIGHT.Pressed = false;
                SHOULDERRIGHT.Tapped = false;
                SHOULDERRIGHT.WasPressed = false;
            }
            #endregion

            #region LEFTANALOG
            LeftAnalogPrevPosition = LeftAnalogCurrentPosition;
            LeftAnalogCurrentPosition = new Vector2(gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y);
            LeftAnalogHover = new Vector2(LeftAnalogPrevPosition.X - LeftAnalogCurrentPosition.X,
                                    LeftAnalogPrevPosition.Y - LeftAnalogCurrentPosition.Y);
            #endregion

            #region RIGHTANALOG
            RightAnalogPrevPosition = RightAnalogCurrentPosition;
            RightAnalogCurrentPosition = new Vector2(gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y);
            RightAnalogHover = new Vector2(RightAnalogPrevPosition.X - RightAnalogCurrentPosition.X,
                                    RightAnalogPrevPosition.Y - RightAnalogCurrentPosition.Y);
            #endregion

            #region LEFTTRIGGER
            LeftShoulderPrevPosition = LeftShoulderCurrentPosition;
            LeftShoulderCurrentPosition = new Vector2(gs.Triggers.Left, gs.Triggers.Left);
            LeftShoulderHover = new Vector2(LeftShoulderPrevPosition.X - LeftShoulderCurrentPosition.X,
                                    LeftShoulderPrevPosition.Y - LeftShoulderCurrentPosition.Y);
            #endregion

            #region RIGHTTRIGGER
            RightShoulderPrevPosition = RightShoulderCurrentPosition;
            RightShoulderCurrentPosition = new Vector2(gs.Triggers.Right, gs.Triggers.Right);
            RightShoulderHover = new Vector2(RightShoulderPrevPosition.X - RightShoulderCurrentPosition.X,
                                    RightShoulderPrevPosition.Y - RightShoulderCurrentPosition.Y);
            #endregion
        }

    }
}
