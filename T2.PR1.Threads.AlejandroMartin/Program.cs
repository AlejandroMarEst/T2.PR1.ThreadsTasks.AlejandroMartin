using System;
using System.Diagnostics;

namespace T2.PR1.Threads.AlejandroMartin
{
    class Program
    {
        public static readonly object ConsoleLocker = new object();
        public static Stopwatch SimulationTime = new Stopwatch();
        public static Random Rng = new Random();
        public static readonly object Chopstick1 = new object();
        public static readonly object Chopstick2 = new object();
        public static readonly object Chopstick3 = new object();
        public static readonly object Chopstick4 = new object();
        public static readonly object Chopstick5 = new object();
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
            double maxHunger = 0;
            Stopwatch hunger = new Stopwatch();
            int timesEaten = 0;
            while(SimulationTime.ElapsedMilliseconds <= 30000)
            {
                Write(num, ConsoleColor.DarkBlue, "Thinking");
                Thread.Sleep(Rng.Next(500, 2001));
                try
                {
                    switch (num)
                    {
                        case 1:
                            Eat(num, Chopstick1, Chopstick2, ref hunger, ref maxHunger, ref timesEaten);
                            break;
                        case 2:
                            Eat(num, Chopstick2, Chopstick3, ref hunger, ref maxHunger, ref timesEaten);
                            break;
                        case 3:
                            Eat(num, Chopstick3, Chopstick4, ref hunger, ref maxHunger, ref timesEaten);
                            break;
                        case 4:
                            Eat(num, Chopstick4, Chopstick5, ref hunger, ref maxHunger, ref timesEaten);
                            break;
                        case 5:
                            Eat(num, Chopstick5, Chopstick1, ref hunger, ref maxHunger, ref timesEaten);
                            break;
                    }
                }
                catch
                {
                    throw new Exception($"Commensal {num} starved");
                }
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
                    return ConsoleColor.DarkGray;
                case 3:
                    return ConsoleColor.Black;
                case 4:
                    return ConsoleColor.DarkMagenta;
                case 5:
                    return ConsoleColor.Gray;
                default:
                    return ConsoleColor.DarkRed;
            }
        }
        private static void Eat(int num, object pal1, object pal2, ref Stopwatch hunger, ref double maxHunger, ref int timesEaten)
        {
            Write(num, ConsoleColor.Yellow, "Grabbing first chopstick");
            lock (pal1)
            {
                Write(num, ConsoleColor.DarkYellow, "Grabbing second chopstick");
                lock (pal2)
                {
                    hunger.Stop();
                    if (hunger.ElapsedMilliseconds > maxHunger)
                    {
                        maxHunger = hunger.ElapsedMilliseconds;
                    }
                    if (hunger.ElapsedMilliseconds > 15000)
                    {
                        throw new Exception();
                    }
                    Write(num, ConsoleColor.Green, "Eating");
                    timesEaten++;
                    Thread.Sleep(Rng.Next(500, 1001));
                    hunger.Restart();
                    Write(num, ConsoleColor.DarkBlue, "Put down first chopstick");
                }
                Write(num, ConsoleColor.DarkCyan, "Put down second chopstick");
            }
        }
    }
}