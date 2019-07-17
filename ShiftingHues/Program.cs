using System;

namespace ShiftingHues
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			//PlayingWithJSON playingWithJSON = new PlayingWithJSON(@"C:\Users\jmort\source\repos\ShiftingHues\ShiftingHues\Spritesheet - PC-Idle NoBorder2.json");
            using (var game = new Game1())
                game.Run();
        }
    }
}
