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
        InputStateManager ism;
        List<Input.Events.Event> lstEvents;
        RealKeyboardState realkeyboardstate;

        public InputManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            ism = new InputStateManager(game);
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ism.Initialize();
            base.Initialize();
            lstEvents = new List<RagadesCubeWin.Input.Events.Event>();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ism.Update(gameTime);

            // NEED TO IMPLEMENT
            List<Input.Events.KeyboardEvent> lstkeyboard =  ism.getRealKeyboardStateEvents();


            foreach (Input.Events.KeyboardEvent ek in lstkeyboard)
            {
                foreach (Input.Events.Event e in lstEvents)
                {
                    // this will be changed in due time in order to flex for all kinds of events
                    e.execute(ek.getKey(),ek.getEvent());
                }
            }

            base.Update(gameTime);
        }



        public void AddEvent(Keys key, Input.Types.EventTypes eventtype, Input.Events.keyboardevent kbevent)
        {
            Input.Events.KeyboardEvent kbe = new Input.Events.KeyboardEvent(key, eventtype, kbevent);

            lstEvents.Add(kbe);
        }
    }
}


