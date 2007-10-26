using System;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GameLogic.Rules
{
    public class RCDefaultGameRules : IRCGameRules
    {
        private RCGameLogic _logic;
        private bool _winnerFound;
        private RCPlayerIndex _winnerIndex;

        public RCDefaultGameRules(RCGameLogic logic)
        {
            _logic = logic;
            _winnerFound = false;
        }

        public void Reset()
        {
            _winnerFound = false;
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

        public void Update(GameTime gameTime)
        {
            _winnerFound = false;

            foreach (IRCGamePlayerViewer player in _logic.GetPlayers())
            {
                if (player.CubeView.IsSolved)
                {
                    _winnerFound = true;
                    _winnerIndex = player.Index;
                    break;
                }
            }
        }

        public bool GetWinner(out RCPlayerIndex index)
        {
            index = _winnerIndex;
            return _winnerFound;
        }
    }
}
