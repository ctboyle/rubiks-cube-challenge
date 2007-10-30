using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

using RagadesCubeWin.Input;
using RagadesCubeWin.Input.Watchers;
using RagadesCubeWin.Input.Types;
using RagadesCubeWin.Input.Events;
using Microsoft.Xna.Framework;


namespace RagadesCubeWin.States.MainMenu
{
    class RCMainMenuSceneInputScheme : RCInputScheme<RCMenuCubeScene>
    {
        public RCMainMenuSceneInputScheme()
            : base()
        {
        }

        void MoveCameraBottom()
        {
            ControlItem.MoveCamera(
                RCMenuCameraController.CameraPositions.Bottom
                );
        }

        void MoveCameraLeft()
        {
            ControlItem.MoveCamera(
                RCMenuCameraController.CameraPositions.Left
                );
        }

        void MoveCameraRight()
        {
            ControlItem.MoveCamera(
                RCMenuCameraController.CameraPositions.Right
                );
        }

        void MoveCameraTop()
        {
            ControlItem.MoveCamera(
                RCMenuCameraController.CameraPositions.Top
                );
        }

        void MoveCameraHome()
        {
            ControlItem.MoveCamera(
                RCMenuCameraController.CameraPositions.Home
                );
        }




        protected override IWatcher[] MapWatcherEvents()
        {
            KeyboardWatcher keyWatcher = new KeyboardWatcher();
            XBox360GamePad gamePadWatcher = new XBox360GamePad(PlayerIndex.One);

            List<IWatcher> mappedWatchers = new List<IWatcher>();

            
            if (keyWatcher.DetectMyInput())
            {

                keyWatcher.WatchEvent(new KeyboardEvent(
                    Keys.Up,
                    EventTypes.OnDown,
                    MoveCameraTop
                    ));

                keyWatcher.WatchEvent(new KeyboardEvent(
                    Keys.Down,
                    EventTypes.OnDown,
                    MoveCameraBottom
                    ));

                keyWatcher.WatchEvent(new KeyboardEvent(
                    Keys.Left,
                    EventTypes.OnDown,
                    MoveCameraLeft
                    ));

                keyWatcher.WatchEvent(new KeyboardEvent(
                    Keys.Right,
                    EventTypes.OnDown,
                    MoveCameraRight
                    ));

                keyWatcher.WatchEvent(new KeyboardEvent(
                    Keys.Enter,
                    EventTypes.OnDown,
                    MoveCameraHome
                    ));

                



                mappedWatchers.Add(keyWatcher);
            }

            /*if (gamePadWatcher.DetectMyInput())
            {
                gamePadWatcher.WatchEvent(new XBox360GamePadEvent(
                   XBox360GamePadTypes.LEFTANALOG,
                   EventTypes.Leaned,
                   LeftAnalog
                   ));

                

                

                mappedWatchers.Add(gamePadWatcher);
            }*/

            return mappedWatchers.ToArray();


        }
    }
}
