using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GameLogic.Rules
{
    public interface IRCGameRules 
    {
        void Reset();
        void Update(GameTime gameTime);
        bool PlayerMoveCube(RCPlayerIndex index);
        bool PlayerSelectCube(RCPlayerIndex index);
        bool PlayerRotateCube(RCPlayerIndex index);
        bool GetWinner(out RCPlayerIndex index);
    }
}
