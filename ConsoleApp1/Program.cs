using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        private static int _counter = 0;
        private static void DoCount()
        {
            int _internalCount = 0;
            for (int i = 0; i < 100000; i++)
            {
                _internalCount++;
            }
            _counter += _internalCount;
        }
        static void Main(string[] args)
        {
            int threadsCount = 100;
			Thread[] threads = new Thread[threadsCount];

			//Allocate threads
			for (int i = 0; i < threadsCount; i++)
				threads[i] = new Thread(DoCount);

			//Start threads
			for (int i = 0; i < threadsCount; i++)
				threads[i].Start();

			//Wait for threads to finish
			for (int i = 0; i < threadsCount; i++)
				threads[i].Join();

			//Print counter
			Console.WriteLine("Counter is: {0}", _counter);
			Console.ReadLine();
        }
    }
}
