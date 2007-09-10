using System;

namespace Ragade_sCubeWin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RagadesCube game = new RagadesCube())
            {
                game.Run();
            }
        }
    }
}

