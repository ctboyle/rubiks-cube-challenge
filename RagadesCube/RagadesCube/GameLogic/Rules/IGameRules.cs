using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RagadesCube.GameLogic.Rules
{
    public interface IRCGameRules 
    {
        void Reset(RCGameLogic logic);
        void Stop();
        void Update(GameTime gameTime);
        bool PlayerMoveCube(RCPlayerIndex index);
        bool PlayerSelectCube(RCPlayerIndex index);
        bool PlayerRotateCube(RCPlayerIndex index);
        bool PlayerOrientCube(RCPlayerIndex index);
        bool IsWinnerPresent { get; }
        IRCCubeShuffer CubeShuffler { get; }
    }
}
