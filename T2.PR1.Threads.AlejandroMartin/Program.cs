using System;
using System.Diagnostics;

namespace T2.PR1.Threads.AlejandroMartin
{
    class Program
    {
        public static readonly object ConsoleLocker = new object();
        public static Stopwatch SimulationTime = new Stopwatch();
        public static Random Rng = new Random();
        public static void Main()
        {
            SimulationTime.Start();
            Thread commensal1 = new Thread(() => Commensal(1));
            Thread commensal2 = new Thread(() => Commensal(2));
            Thread commensal3 = new Thread(() => Commensal(3));
            Thread commensal4 = new Thread(() => Commensal(4));
            Thread commensal5 = new Thread(() => Commensal(5));
            commensal1.Start();
            commensal2.Start();
            commensal3.Start();
            commensal4.Start();
            commensal5.Start();
        }
        private static void Commensal(int num)
        {
            Stopwatch hunger = new Stopwatch();
            int timesEaten = 0;
            while(SimulationTime.ElapsedMilliseconds <= 30000)
            {
                Write(num, ConsoleColor.DarkBlue, "Thinking");
                Thread.Sleep(Rng.Next(500, 2001));
            }
        }

        private static void Write(int num, ConsoleColor color, string msg)
        {
            lock (ConsoleLocker)
            {
                Console.ForegroundColor = GetColor(num);
                Console.BackgroundColor = color;
                Console.WriteLine($"{num}: {msg}[{DateTime.Now:HH:mm:ss}]");
                Console.ResetColor();
            }
        }

        private static ConsoleColor GetColor(int numCommensal)
        {
            switch (numCommensal)
            {
                case 1:
                    return ConsoleColor.Red;
                case 2:
                    return ConsoleColor.Yellow;
                case 3:
                    return ConsoleColor.Blue;
                case 4:
                    return ConsoleColor.White;
                case 5:
                    return ConsoleColor.DarkCyan;
                default:
                    return ConsoleColor.DarkRed;
            }
        }
    }
}