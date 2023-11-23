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

                    var input = Convert.ToInt32(Console.ReadLine());

                    switch (input)
                    {
                        case 1:
                            Console.WriteLine("Task Completed Successfully");
                            break;
                        case 2:
                            Console.WriteLine("Task Faulted");
                            throw new Exception("Task Faulted");
                        case 3:
                            Console.WriteLine("Task Cancelled");
                            tokenSource2.Cancel();
                            break;
                        default:
                            Console.WriteLine("Invalid Input!!!");
                            break;
                    }

                    try
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException e)
                    {
                        Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
                    }


                }, ct);

                antecedent.ContinueWith(t => Console.WriteLine("Continuation-1 Executed"), TaskContinuationOptions.None);
                antecedent.ContinueWith(t => Console.WriteLine("Continuation-2 Executed: Antecedent NotRanToCompletion"), TaskContinuationOptions.NotOnRanToCompletion);
                antecedent.ContinueWith(t => Console.WriteLine("Continuation-3 Executed: Antecedent Faulted"), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
                antecedent.ContinueWith(t =>
                {
                    Console.WriteLine("Continuation-4 Executed: Antecedent Canceled");
                    tokenSource2.Dispose();
                    tokenSource2 = new CancellationTokenSource();
                    ct = tokenSource2.Token;
                }, TaskContinuationOptions.OnlyOnCanceled);

                try
                {
                    antecedent.Wait();
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
