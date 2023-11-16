/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Random random = new Random();

            Task<Int32[]> arrayTask = Task.Run(() => {
                return generateArray(10, random);
            });

            Task<Int32[]> multiplyTask = arrayTask.ContinueWith<Int32[]>((antecedent) =>
            {
                int rndInt = getRandomInteger(random);
                return antecedent.Result.Select(x => x * rndInt).ToArray();
            });

            Task<Int32[]> sortTask = multiplyTask.ContinueWith<Int32[]>((antecedent) =>
            {
                Array.Sort(antecedent.Result);
                return antecedent.Result;
            });

            Task<Double> avgTask = sortTask.ContinueWith<Double>((antecedent) =>
            {
                return antecedent.Result.Average();
            });

            Console.WriteLine("Random Array: [{0}]", string.Join(", ", arrayTask.Result));

            Console.WriteLine("Multiplied Array: [{0}]", string.Join(", ", multiplyTask.Result));

            Console.WriteLine("Sorted Array: [{0}]", string.Join(", ", sortTask.Result));

            Console.WriteLine("Avg: {0}", avgTask.Result);

            Console.ReadLine();
        }

        public static int[] generateArray(int count, Random random)
        {
            int[] values = new int[count];

            for (int i = 0; i < count; ++i)
                values[i] = getRandomInteger(random);

            return values;
        }

        public static int getRandomInteger(Random random)
        {
            return random.Next();
        }
    }
}
