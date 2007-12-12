#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace RC.Engine.Input
{
    /// <summary>
    /// Input manager that will handle delegates and events
    /// </summary>
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {


        List<IWatcher> watchers;                    // List of watchers


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

            // run through each watcher and allow them to 
            //  check their events
            foreach (IWatcher w in watchers)
            {
                w.RunEvents();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Add a watcher to the manager
        /// </summary>
        /// <param name="watcher">Watcher to add</param>
        public void AddWatcher(IWatcher watcher)
        {
            watchers.Add(watcher);
        }

        /// <summary>
        /// Remove a watcher from prefix
        /// </summary>
        /// <param name="index">Which player</param>
        public void RemoveWatcher(IWatcher watcher)
        {
            watchers.Remove(watcher);
        }
    }
}


