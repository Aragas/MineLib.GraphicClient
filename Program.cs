using System;

namespace MineLib.GraphicClient
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameClient())
            {
                game.Run();
                game.Exit();
            }

        }
    }
#endif
}
