#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace RagadesCubeWin.Input
{
   
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputStateManager : Microsoft.Xna.Framework.GameComponent
    {
        #region GAMEPADSTATES
        KeyboardState keyboardstate;
        MouseState mousestate;
        GamePadState[] gamepadstate;

        #endregion

        #region REAL INPUT STATES
        private RealKeyboardState realkeyboardstate;
        #endregion

        public InputStateManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            keyboardstate = new KeyboardState();
            mousestate = new MouseState();
            gamepadstate = new GamePadState[4];

            gamepadstate[0] = new GamePadState();
            gamepadstate[1] = new GamePadState();
            gamepadstate[2] = new GamePadState();
            gamepadstate[3] = new GamePadState();
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            realkeyboardstate = new RealKeyboardState(); 
            base.Initialize();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update States Here
            keyboardstate = Keyboard.GetState();
            mousestate = Mouse.GetState();
                      
            gamepadstate[0] = GamePad.GetState(PlayerIndex.One);
            gamepadstate[1] = GamePad.GetState(PlayerIndex.Two);
            gamepadstate[2] = GamePad.GetState(PlayerIndex.Three);
            gamepadstate[3] = GamePad.GetState(PlayerIndex.Four);


            // use custom state managers here
            realkeyboardstate.KeyboardState(keyboardstate);

            base.Update(gameTime);
        }

        public List<Input.Events.KeyboardEvent> getRealKeyboardStateEvents()
        {
            return realkeyboardstate.eventList();
        }
    }
}


