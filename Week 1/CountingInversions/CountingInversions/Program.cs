using System;
using System.IO;
using System.Linq;

namespace CountingInversions
{
    internal class Program
    {
        private static void Main()
        {
            long inversions = 0;
            var array = File.ReadAllLines(@"..\..\IntegerArray.txt").Select(long.Parse).ToArray();

            MergeSort(array, ref inversions);

            Console.WriteLine(inversions);
            Console.ReadLine();
        }

        private static long[] MergeSort(long[] array, ref long inversions)
        {
            if (array.Length <= 1) return array;

            var left = new long[array.Length/2];
            var right = new long[array.Length - left.Length];

            Array.Copy(array, 0, left, 0, left.Length);
            Array.Copy(array, array.Length/2, right, 0, right.Length);

            left = MergeSort(left, ref inversions);
            right = MergeSort(right, ref inversions);

            return Merge(left, right, ref inversions);
        }

        private static long[] Merge(long[] left, long[] right, ref long inversions)
        {
            var array = new long[left.Length + right.Length];
            long j = 0, k = 0;

            for (long i = 0; i < array.Length; i++)
            {
                if (j < left.Length && k < right.Length)
                {
                    if (left[j] < right[k])
                    {
                        array[i] = left[j++];
                    }
                    else
                    {
                        array[i] = right[k++];
                        inversions += (left.Length - j);
                    }
                }
                else if (j < left.Length)
                {
                    array[i] = left[j++];
                }
                else
                {
                    array[i] = right[k++];
                }
            }

            return array;
        }
    }
}
