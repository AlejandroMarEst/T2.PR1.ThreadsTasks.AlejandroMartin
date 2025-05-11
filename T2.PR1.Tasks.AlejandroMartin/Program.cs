using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace T2.PR1.Tasks.AlejandroMartin
{
    class Program
    {
        public static Entity windowSize = new Entity { CoordX = 60, CoordY = 30 };
        public static Stopwatch sw = new Stopwatch();
        public static Entity spaceship = new Entity { CoordX = windowSize.CoordX/2, CoordY = windowSize.CoordY - 1 };
        public static bool gameover = false;
        public static bool quit = false;
        public readonly static object _locker = new object();
        public static List<Entity> meteorites = new List<Entity>();
        public static Records stats = new Records { Score = 0, TimeSurvived = 0};
        public static async Task Main()
        {
            sw.Start();
            LoadConfig();
            Task.Run(UpdateAsteroids);
            Task.Run(ControllSpaceship);
            while (!quit)
            {
                stats.DateGameSession = DateTime.Now;
                while (!gameover)
                {
                    DrawGame();
                    await Task.Delay(50);
                }
                stats.TimeSurvived = sw.ElapsedMilliseconds / 1000;
                SaveRecord();
                Console.Clear();
                Console.WriteLine($"Asteroids dodged: {stats.Score}\nTotal survival time: {stats.TimeSurvived}\nTry again? Y/N");
                string userChoice = Console.ReadLine();
                if(userChoice.ToUpper() == "NO" || userChoice.ToUpper() == "N")
                {
                    quit = true;
                } else { 
                    gameover = false;
                    stats.Score = 0;
                    stats.TimeSurvived = 0;
                    sw.Restart();
                }
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
                foreach (var meteorite in meteorites)
                {
                    Console.SetCursorPosition(meteorite.CoordX, meteorite.CoordY);
                    Console.Write("#");
                }
            }
        }
        public static async Task ControllSpaceship()
        {
            while (!quit)
            {
                while (!gameover)
                {
                    var input = Console.ReadKey(true).Key;
                    if (input == ConsoleKey.A && spaceship.CoordX != 0)
                    {
                        spaceship.CoordX--;
                    }
                    if (input == ConsoleKey.D && spaceship.CoordX != windowSize.CoordX - 1)
                    {
                        spaceship.CoordX++;
                    }
                    if (input == ConsoleKey.Q)
                    {
                        sw.Stop();
                        gameover = true;
                    }
                    await Task.Delay(50);
                }
            }
        }
        public static void SaveRecord()
        {
            string filePath = "../../../records.csv";
            using StreamWriter sw = new StreamWriter(filePath, true);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !File.Exists(filePath)
            };
            using (var csvWriter = new CsvWriter(sw, config))
            {

                csvWriter.WriteRecord(stats);
                csvWriter.NextRecord();
            }
        }
        public static async Task UpdateAsteroids()
        {
            Random rng = new Random();
            while (!quit)
            {
                while (!gameover)
                {
                    lock (_locker)
                    {
                        meteorites.Add(new Entity { CoordX = rng.Next(Console.WindowWidth), CoordY = 0 });
                        for (int i = 0; i < meteorites.Count; i++)
                        {
                            meteorites[i].CoordY += 1;
                        }
                        meteorites.ForEach(a =>
                        {
                            if (a.CoordY >= Console.WindowHeight)
                            {
                                stats.Score++;
                            }
                        });
                        meteorites.RemoveAll(a => a.CoordY >= Console.WindowHeight);
                        if (meteorites.Any(a => a.CoordX == spaceship.CoordX && a.CoordY == spaceship.CoordY))
                        {
                            spaceship.CoordX = 30;
                            spaceship.CoordY = 29;
                            meteorites.Clear();
                            gameover = true;
                        }
                    }
                    await Task.Delay(20);
                }
            }
        }
    }
}