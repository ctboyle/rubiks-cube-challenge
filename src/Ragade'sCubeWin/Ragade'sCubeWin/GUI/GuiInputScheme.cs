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
        public GuiInputScheme(InputManager im)
            : base(im)
        {
        }


        private void OnKeyDown(Keys key)
        {
            GUIMoveEvent.GUIMoveDirection dir = GUIMoveEvent.GUIMoveDirection.Down;
            bool fMoveKey = false;
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
                default:
                    break;
            }

            if (fMoveKey)
            {
                _cntrlItem.GuiInputEvent(new GUIMoveEvent(
                dir,
                _cntrlItem
                ));
            }
            else
            {
                _cntrlItem.GuiInputEvent(new GUIKeyEvent(
                    key,
                    _cntrlItem
                    ));
            }
        }

        private void OnMouseDown(Vector2 position, Vector2 move)
        {
            _cntrlItem.GuiInputEvent(new GUIMouseEvent(
                GUIMouseEvent.GUIMouseEventType.MouseDown,
                (int)position.X,
                (int)position.Y,
                _cntrlItem
                ));
        }

        private void OnMouseUp(Vector2 position, Vector2 move)
        {
            _cntrlItem.GuiInputEvent(new GUIMouseEvent(
                GUIMouseEvent.GUIMouseEventType.MouseUp,
                (int)position.X,
                (int)position.Y,
                _cntrlItem
                ));
        }

        protected override IWatcher[] MapWatcherEvents(RCGUIManager cntrlItem)
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();
            MouseWatcher mouseWatcher = new MouseWatcher();
            IWatcher[] mappedWatchers = new IWatcher[]
            {
                keyWatcher,
                mouseWatcher
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
                

            return mappedWatchers;

        }
    }
}
