using System;
using System.Threading;

namespace ComputerController
{
    class Program
    {
        public static string Directory = AppDomain.CurrentDomain.BaseDirectory;

        public static bool isRunning = false;
        public static int Port = ;//put port fowarded port here

        static void Main(string[] args)
        {
            Console.Title = "Computer Controller 1.0";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(Port);

            Console.WriteLine("To use commands use '/' followed by the command. /commandlist for a list of commands.");
            Commands.ListenForConsoleInput();
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main Thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    // If the time for the next loop is in the past, aka it's time to execute another tick
                    ThreadManager.UpdateMain();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK); // Calculate at what point in time the next tick should be executed

                    if (_nextLoop > DateTime.Now)
                    {
                        // If the execution time for the next tick is in the future, aka the server is NOT running behind
                        Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
                    }
                }
            }
        }
    }
}
