using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace T2.PR1.Tasks.AlejandroMartin
{
    class Program
    {
        public static Entity windowSize = new Entity { CoordX = 60, CoordY = 30 };
        public static Stopwatch sw = new Stopwatch();
        public static Entity spaceship = new Entity { CoordX = windowSize.CoordX/2, CoordY = windowSize.CoordY - 1 };
        public static bool gameover = false;
        public static object _locker = new object();
        public static async Task Main()
        {
            sw.Start();
            LoadConfig();
            Task.Run(ControllSpaceship);
            while (!gameover)
            {
                DrawGame();
                await Task.Delay(10);
            }
        }
        public static void LoadConfig()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(windowSize.CoordX, windowSize.CoordY);
        }
        public static void DrawGame()
        {
            lock (_locker)
            {
                Console.Clear();
                Console.SetCursorPosition(spaceship.CoordX, spaceship.CoordY);
                Console.Write("^");
            }
        }
        public static async Task ControllSpaceship()
        {
            while (!gameover)
            {
                var input = Console.ReadKey(true).Key;
                if(input == ConsoleKey.A && spaceship.CoordX != 0)
                {
                    spaceship.CoordX--;
                }
                if(input == ConsoleKey.D && spaceship.CoordX != windowSize.CoordX - 1){
                    spaceship.CoordX++;
                }
                if(input == ConsoleKey.Q)
                {
                    sw.Stop();
                    gameover = true;
                }
                await Task.Delay(50);
            }
        }
    }
}