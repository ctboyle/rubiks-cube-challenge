using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;



namespace RagadesCubeWin.GUI
{
    class GuiInputScheme : RCInputScheme<RCGUIManager>
    {
        enum AnalogstickState
        {
            AboveThreshold,
            UnderThreshold
        }

        private AnalogstickState _lastLeftAnalogState;

        public GuiInputScheme(InputManager im)
            : base(im)
        {
            _lastLeftAnalogState = AnalogstickState.UnderThreshold;
        }


        private void OnKeyDown(Keys key)
        {
            GUIMoveEvent.GUIMoveDirection dir = GUIMoveEvent.GUIMoveDirection.Down;
            GUISelectEvent.GUISelectType select = GUISelectEvent.GUISelectType.Ok;
            bool fMoveKey = false;
            bool fSelectKey = false;
            switch (key)
            {
                case Keys.Down:
                    dir = GUIMoveEvent.GUIMoveDirection.Down;
                    fMoveKey = true;
                    break;
                case Keys.Up:
                    dir = GUIMoveEvent.GUIMoveDirection.Up;
                    fMoveKey = true;
                    break;
                case Keys.Left:
                    dir = GUIMoveEvent.GUIMoveDirection.Left;
                    fMoveKey = true;
                    break;
                case Keys.Right:
                    dir = GUIMoveEvent.GUIMoveDirection.Right;
                    fMoveKey = true;
                    break;
                case Keys.Enter:
                    select = GUISelectEvent.GUISelectType.Ok;
                    fSelectKey = true;
                    break;
                case Keys.Escape:
                    select = GUISelectEvent.GUISelectType.Cancel;
                    fSelectKey = true;
                    break;
                default:
                    break;
            }

            if (fMoveKey)
            {
                ControlItem.GuiInputEvent(new GUIMoveEvent(
                    dir,
                    ControlItem
                    ));
            }
            if (fSelectKey)
            {
                ControlItem.GuiInputEvent(new GUISelectEvent(
                    select,
                    ControlItem
                    ));
            }
            else
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

            if (Vector2.Dot(position, Vector2.UnitY) >= 0.90f)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Up;
                stickState = AnalogstickState.AboveThreshold;
            }
            else if (Vector2.Dot(position, -Vector2.UnitY) >= 0.90f)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Down;
                stickState = AnalogstickState.AboveThreshold;
            }
            else if (Vector2.Dot(position, Vector2.UnitX) >= 0.90f)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Right;
                stickState = AnalogstickState.AboveThreshold;
            }
            else if (Vector2.Dot(position, -Vector2.UnitX) >= 0.90f)
            {
                stickDir = GUIMoveEvent.GUIMoveDirection.Left;
                stickState = AnalogstickState.AboveThreshold;
            }

            // Prevent repeat events
            if (stickState == AnalogstickState.AboveThreshold &&
                _lastLeftAnalogState == AnalogstickState.UnderThreshold)
            {
                ControlItem.GuiInputEvent(new GUIMoveEvent(
                    stickDir,
                    ControlItem
                    ));
            }

            _lastLeftAnalogState = stickState;
        }

        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();
            MouseWatcher mouseWatcher = new MouseWatcher();
            XBox360GamePad gamePadWatcher = new XBox360GamePad(PlayerIndex.One);

            IWatcher[] mappedWatchers = new IWatcher[]
            {
                keyWatcher,
                mouseWatcher,
                gamePadWatcher
            };

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

            keyWatcher.WatchEvent(new KeyboardEvent(
                EventTypes.OnDown,
                OnKeyDown
                ));

            gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
               XBox360GamePadTypes.LEFTANALOG,
               EventTypes.Leaned,
               LeftAnalog
               ));

            return mappedWatchers;

        }
    }
}
