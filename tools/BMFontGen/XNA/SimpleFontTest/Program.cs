using System;

namespace SimpleFontTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SimpleFontTest game = new SimpleFontTest())
            {
                game.Run();
            }
        }
    }
}

