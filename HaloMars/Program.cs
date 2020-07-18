using System;

namespace HaloMars
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (Window game = new Window(1000, 600, "Halo tu Mars"))
            {
                game.Run(30.0, 30.0);
            }
        }
    }
}