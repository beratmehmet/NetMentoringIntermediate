/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            while (true)
            {
                Task antecedent = Task.Run(() =>
                {
                    Console.WriteLine("What is the expected behaviour of the antecedent?");
                    Console.WriteLine("[1]Ran to completion [2]Faulted [3]Cancelled");
                    int input = Convert.ToInt32(Console.ReadLine());

                    if (input == 1)
                    {
                        Console.WriteLine("Task Completed Successfully");
                    }
                    else if (input == 2)
                    {
                        Console.WriteLine("Task Faulted");
                        throw new Exception("Task Faulted");
                    }
                    else if (input == 3)
                    {
                        Console.WriteLine("Task Cancelled");
                        tokenSource2.Cancel();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input!!!");
                    }
                    ct.ThrowIfCancellationRequested();
                }, tokenSource2.Token);

                Task continuation1 = antecedent.ContinueWith(t => Console.WriteLine("Continuation-1 Executed"), TaskContinuationOptions.None);
                Task continuation2 = antecedent.ContinueWith(t => Console.WriteLine("Continuation-2 Executed: Antecedent NotRanToCompletion"), TaskContinuationOptions.NotOnRanToCompletion);
                Task continuation3 = antecedent.ContinueWith(t => Console.WriteLine("Continuation-3 Executed: Antecedent Faulted"), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
                Task continuation4 = antecedent.ContinueWith(t => Console.WriteLine("Continuation-4 Executed: Antecedent Canceled"), TaskContinuationOptions.OnlyOnCanceled);

                 antecedent.Wait();
                
            }
        }
    }
}
