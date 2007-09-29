#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RagadesCubeWin.Input.Events;
using RagadesCubeWin.Input.Types;
#endregion

namespace RagadesCubeWin.Input
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {

        List<IWatcher> watchers;
        RealKeyboardState realkeyboardstate;


        public InputManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
         
            base.Initialize();
            watchers = new List<IWatcher>();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            // NEED TO IMPLEMENT
            foreach (IWatcher w in watchers)
            {
                w.RunEvents();
            }

            //foreach (Input.Events.KeyboardEvent ek in lstkeyboard)
            //{
            //    foreach (Input.Events.Event e in lstEvents)
            //    {
            //        // this will be changed in due time in order to flex for all kinds of events
            //        e.execute(ek.getKey(),ek.getEvent());
            //    }
            //}

            base.Update(gameTime);
        }

        public void AddWatcher(IWatcher watcher)
        {
            watchers.Add(watcher);
        }

        public IWatcher getKeyboardWatcher()
        {
            return new Input.Watchers.Keyboard();
        }

        public IWatcher getMouseWatcher()
        {
            return new Input.Watchers.Mouse();
        }
    }
}


