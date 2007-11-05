using System;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GameLogic.Rules
{
    public class RCDefaultGameRules : IRCGameRules
    {
        private readonly DefaultCubeShuffler SHUFFLER = new DefaultCubeShuffler();

        private RCGameLogic _logic;

        public RCDefaultGameRules()
        {
        }

        public void Reset(RCGameLogic logic)
        {
            _logic = logic;
        }

        public void Stop()
        {
            _logic = null;
        }

        public void Update(GameTime gameTime)
        {
        }

        public bool PlayerMoveCube(RCPlayerIndex index)
        {
            return true;
        }

        public bool PlayerSelectCube(RCPlayerIndex index)
        {
            return true;
        }

        public bool PlayerRotateCube(RCPlayerIndex index)
        {
            return true;
        }

        public bool IsWinnerPresent
        {
            get 
            {
                foreach (IRCGamePlayerViewer player in _logic.GetPlayers())
                    if (player.CubeView.IsSolved) 
                        return true;
                return false;
            }
        }

        public IRCCubeShuffer CubeShuffler
        {
            get { return SHUFFLER;  }
        }
    }
}
