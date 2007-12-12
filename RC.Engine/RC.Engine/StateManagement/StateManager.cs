using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RC.Engine.StateManagement
{
    public delegate void StateChangeHandler(RCGameState newState, RCGameState oldState);

    public interface IGameStateManager
    {
        event StateChangeHandler OnStateChange;
        RCGameState State { get; }
        void PopState();
        void PushState(RCGameState state);
        bool ContainsState(RCGameState state);
        void ChangeState(RCGameState newState);
    }


    class RCGameStateManager :  GameComponent, IGameStateManager
    {
        private Stack<RCGameState> states = new Stack<RCGameState>();

        public event StateChangeHandler OnStateChange;

        private int initialDrawOrder = 0;
        private int drawOrder;

        public RCGameStateManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGameStateManager), this);
            drawOrder = initialDrawOrder;
        }

        private RCGameState RemoveState()
        {
            RCGameState oldState = (RCGameState)states.Peek();

            //remove the state from our game components
            Game.Components.Remove(oldState.Value);

            states.Pop();

            return oldState;
        }

        private void AddState(RCGameState state)
        {
            states.Push(state);

            Game.Components.Add(state);

            //Register the event for this state
            OnStateChange += state.StateChanged;
        }

        public void PopState()
        {
            RCGameState oldState = RemoveState();

            drawOrder -= 100;

            //Let everyone know we just changed states
            if (OnStateChange != null)
                OnStateChange(State, oldState);

            //Unregister the event for this state
            OnStateChange -= oldState.StateChanged;
        }

        public void PushState(RCGameState newState)
        {
            RCGameState oldState = State as RCGameState;
            drawOrder += 100;
            newState.DrawOrder = drawOrder;
            
            AddState(newState);

            //Let everyone know we just changed states
            if (OnStateChange != null)
                OnStateChange(newState, oldState);
        }

        public void ChangeState(RCGameState newState)
        {
            //We are changing states, so pop everything ...
            //if we don't want to really change states but just modify,
            //we should call PushState and PopState
            while (states.Count > 0)
            {
                RCGameState oldState = RemoveState();
                if (oldState != null)
                {
                    //Unregister the event for this state
                    OnStateChange -= oldState.StateChanged;
                }
            }

            //changing state, reset our draw order
            newState.DrawOrder = drawOrder = initialDrawOrder;
            AddState(newState);

            //Let everyone know we just changed states
            if (OnStateChange != null)
                OnStateChange(State, null);
        }

        public bool ContainsState(RCGameState state)
        {
            return (states.Contains(state));
        }

        public RCGameState State
        {
            get
            {
                if (states.Count != 0)
                {
                    return (states.Peek());
                }
                return null;
            }
        }


    }
}
