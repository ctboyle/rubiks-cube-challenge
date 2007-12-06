using System;
using System.Collections.Generic;
using System.Text;

using RC.Engine.Input;
using RC.Input.Watchers;
using RC.Input.Events;
using RC.Input.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using RC.Input;



namespace RC.Gui
{
    public class GuiInputScheme : RCInputScheme<RCGUIManager>
    {
        enum AnalogstickState
        {
            AboveThreshold,
            UnderThreshold
        }

        private AnalogstickState _lastLeftAnalogState;

        public GuiInputScheme()
        {
            _lastLeftAnalogState = AnalogstickState.UnderThreshold;
        }


        private void OnKeyDown(Keys key)
        {
            bool fKeyEvent = true;
            switch (key)
            {
                case Keys.Down:
                    ControlItem.MoveDown();
                    break;
                case Keys.Up:
                    ControlItem.MoveUp();
                    break;
                case Keys.Left:
                    ControlItem.MoveLeft();
                    break;
                case Keys.Right:
                    ControlItem.MoveRight();
                    break;
                case Keys.Enter:
                    ControlItem.AcceptFocused();
                    break;
                case Keys.Escape:
                    ControlItem.DeclineFocused();
                    break;
                default:
                    fKeyEvent = true;
                    break;
            }

            if (fKeyEvent)
            {
                ControlItem.GuiInputEvent(new GUIKeyEvent(
                    key,
                    ControlItem
                    ));
            }
        }

        private void OnMouseDown(Vector2 position, Vector2 move)
        {
            ControlItem.GuiInputEvent(new GUIMouseEvent(
                GUIMouseEvent.GUIMouseEventType.MouseDown,
                (int)position.X,
                (int)position.Y,
                ControlItem
                ));
        }

        private void OnMouseUp(Vector2 position, Vector2 move)
        {
            ControlItem.GuiInputEvent(new GUIMouseEvent(
                GUIMouseEvent.GUIMouseEventType.MouseUp,
                (int)position.X,
                (int)position.Y,
                ControlItem
                ));
        }

        private void LeftAnalog(Vector2 position, Vector2 move)
        {
            const float stickThreshold = 0.90f;

            AnalogstickState stickState = AnalogstickState.UnderThreshold;

            // TODO: have NONE direction.
            GUIMoveEvent.GUIMoveDirection stickDir = GUIMoveEvent.GUIMoveDirection.Up;

            if (Vector2.Dot(position, Vector2.UnitY) >= stickThreshold)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Up;
                stickState = AnalogstickState.AboveThreshold;
            }
            else if (Vector2.Dot(position, -Vector2.UnitY) >= stickThreshold)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Down;
                stickState = AnalogstickState.AboveThreshold;
            }
            else if (Vector2.Dot(position, Vector2.UnitX) >= stickThreshold)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Right;
                stickState = AnalogstickState.AboveThreshold;
            }
            else if (Vector2.Dot(position, -Vector2.UnitX) >= stickThreshold)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Left;
                stickState = AnalogstickState.AboveThreshold;
            }

            // Prevent repeat events
            if (stickState == AnalogstickState.AboveThreshold &&
                _lastLeftAnalogState == AnalogstickState.UnderThreshold)
            {
                ControlItem.MoveEvent(stickDir);
            }

            _lastLeftAnalogState = stickState;
        }




        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();
            XBox360GamePad gamePadWatcher = new XBox360GamePad(PlayerIndex.One);

#if !XBOX

            MouseWatcher mouseWatcher = new MouseWatcher();
#endif

            List<IWatcher> mappedWatchers = new List<IWatcher>();

#if !XBOX

            if (mouseWatcher.DetectMyInput())
            {
                mouseWatcher.WatchEvent(new MouseEvent(
                    MouseInput.LeftButton,
                    EventTypes.OnDown,
                    OnMouseDown
                    ));

                mouseWatcher.WatchEvent(new MouseEvent(
                    MouseInput.LeftButton,
                    EventTypes.OnUp,
                    OnMouseUp
                    ));
                
                mappedWatchers.Add(mouseWatcher);
            }

#endif
            if (keyWatcher.DetectMyInput())
            {

                keyWatcher.WatchEvent(new KeyboardEvent(
                    EventTypes.OnDown,
                    OnKeyDown
                    ));

                mappedWatchers.Add(keyWatcher);
            }

            if (gamePadWatcher.DetectMyInput())
            {
                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.LEFTANALOG,
                   EventTypes.Leaned,
                   LeftAnalog
                   ));

                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.A,
                   EventTypes.OnDown,
                   ControlItem.AcceptFocused
                   ));

                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.B,
                   EventTypes.OnDown,
                   ControlItem.DeclineFocused
                   ));

                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.DDOWN,
                   EventTypes.OnDown,
                   ControlItem.MoveDown
                   ));

                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.DUP,
                   EventTypes.OnDown,
                   ControlItem.MoveUp
                   ));

                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.DLEFT,
                   EventTypes.OnDown,
                   ControlItem.MoveLeft
                   ));

                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.DRIGHT,
                   EventTypes.OnDown,
                   ControlItem.MoveRight
                   ));

                mappedWatchers.Add(gamePadWatcher);
            }

            return mappedWatchers.ToArray();

        }
    }
}
