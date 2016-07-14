using System;
using System.IO;
using System.Linq;

namespace Quicksort
{
    internal class Program
    {
        private static void Main()
        {
            var pivots = new Action<long[], long, long>[]
            {
                (a, i, j) => { },
                (a, i, j) => { Swap(a, i, j); },
                (a, i, j) =>
                {
                    var k = i + (j - i)/2;
                    var max = Math.Max(a[i], Math.Max(a[j], a[k]));
                    var min = Math.Min(a[i], Math.Min(a[j], a[k]));

                    if (a[i] != max && a[i] != min) k = i;
                    else if (a[j] != max && a[j] != min) k = j;
                    Swap(a, i, k);
                }
            };

            foreach (var pivot in pivots)
            {
                var array = File.ReadAllLines(@"..\..\QuickSort.txt").Select(long.Parse).ToArray();
                long comparisons = 0;

                Quicksort(array, 0, array.Length - 1, pivot, ref comparisons);
                Console.WriteLine(comparisons);
            }

            Console.ReadLine();
        }

        private static void Quicksort(long[] array, long left, long right, Action<long[], long, long> pivot, ref long comparisons)
        {
            if (left < right)
            {
                var p = Partition(array, left, right, pivot);

                comparisons += (right - left);

                Quicksort(array, left, p - 1, pivot, ref comparisons);
                Quicksort(array, p + 1, right, pivot, ref comparisons);
            }
        }

        private static long Partition(long[] array, long left, long right, Action<long[], long, long> pivot)
        {
            pivot(array, left, right);
            var p = array[left];
            var i = left + 1;

            for (var j = left + 1; j <= right; j++)
            {
                if (array[j] < p)
                {
                    Swap(array, j, i);
                    i++;
                }
            }

            Swap(array, left, i - 1);
            return i - 1;
        }

        private static void Swap(long[] array, long i, long j)
        {
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
