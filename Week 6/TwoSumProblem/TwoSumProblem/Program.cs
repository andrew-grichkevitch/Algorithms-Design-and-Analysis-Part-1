using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TwoSumProblem
{
    internal class Program
    {
        private static void Main()
        {
            var array = ReadData(@"..\..\algo1-programming_prob-2sum.txt");
            Array.Sort(array);

            int count = 0;
            for (var t = -10000; t <= 10000; t++)
            {
                if (TwoSumExists(array, t)) count++;
            }
            Console.WriteLine(count);

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        private static long[] ReadData(string path)
        {
            var numbers = new List<long>();

            foreach (var line in File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)))
            {
                numbers.Add(long.Parse(line));
            }

            return numbers.ToArray();
        }

        private static bool TwoSumExists(long[] array, long t)
        {
            long i = 0, j = array.Length - 1;

            while (i < j)
            {
                if (array[i] == array[j]) return false;
                var sum = array[i] + array[j];

                if (sum == t) return true;
                else if (sum < t) i++;
                else j--;
            }

            return false;
        }
    }
}
