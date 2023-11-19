/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> sharedCollection = new List<int>();
        private static AutoResetEvent itemAddedEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Thread addingThread = new Thread(AddElement);
            Thread printingThread = new Thread(PrintElement);

            addingThread.Start();
            printingThread.Start();

            Console.ReadLine();
        }

        private static void PrintElement(object obj)
        {
            for(int i = 0; i < 10; i++)
            {
                itemAddedEvent.WaitOne();
                Console.WriteLine("Shared Collection Items: [{0}]", string.Join(", ", sharedCollection));
                
            }
        }

        private static void AddElement(object obj)
        {
            for (int i = 0; i < 10; i++) 
            {
                Thread.Sleep(1000);
                sharedCollection.Add(i);
                itemAddedEvent.Set();
            }
        }
    }
}
