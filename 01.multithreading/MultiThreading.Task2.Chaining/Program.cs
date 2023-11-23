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

            var arrayTask = Task.Run(() =>
            {
                var randomArray = GenerateArray(10, random);
                Console.WriteLine("Random Array: [{0}]", string.Join(", ", randomArray));

                return randomArray;
            }).ContinueWith((antecedent) =>
            {
                int randomInt = GetRandomInteger(random);
                var multipliedArray = antecedent.Result.Select(x => x * randomInt).ToArray();
                Console.WriteLine("Multiplied Array: [{0}]", string.Join(", ", multipliedArray));

                return multipliedArray;
            }).ContinueWith((antecedent) =>
            {
                Array.Sort(antecedent.Result);
                Console.WriteLine("Sorted Array: [{0}]", string.Join(", ", antecedent.Result));

                return antecedent.Result;
            }).ContinueWith((antecedent) =>
            {
                var average = antecedent.Result.Average();
                Console.WriteLine("Avg: {0}", average);

                return average;
            });

            Console.ReadLine();
        }

        public static int[] GenerateArray(int count, Random random)
        {
            var values = new int[count];

            for (int i = 0; i < count; ++i)
                values[i] = GetRandomInteger(random);

            return values;
        }

        public static int GetRandomInteger(Random random) => random.Next();
    }
}
