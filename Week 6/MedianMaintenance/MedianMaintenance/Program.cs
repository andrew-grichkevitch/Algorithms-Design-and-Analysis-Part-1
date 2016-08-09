using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MedianMaintenance
{
    internal class Program
    {
        private static void Main()
        {
            var array = ReadData(@"..\..\Median.txt");
            var medians = new long[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                var heap = new MedianHeap();

                for (int j = 0; j <= i; j++)
                {
                    heap.Push(array[j]);
                }

                medians[i] = heap.Pop();
            }

            Console.WriteLine(medians.Sum(e => e) % array.Length);

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
    }

    internal abstract class Heap
    {
        protected int CompareTo;

        private readonly List<long> elements;

        public long Count { get { return elements.Count; } }

        public Heap()
        {
            elements = new List<long>();
        }

        public long Get()
        {
            return elements[0];
        }

        public long Pop()
        {
            var item = elements[0];
            elements[0] = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);
            BubbleDown();

            return item;
        }

        public void Push(long item)
        {
            elements.Add(item);
            BubbleUp();
        }


        private void BubbleDown()
        {
            int current = 0;

            int? rightChild = (current + 1) * 2;
            int? leftChild = (current + 1) * 2 - 1;
            if (leftChild >= elements.Count) leftChild = null;
            if (rightChild >= elements.Count) rightChild = null;

            while (leftChild != null && (elements[current].CompareTo(elements[leftChild.Value]) == -CompareTo
               || (rightChild != null && elements[current].CompareTo(elements[rightChild.Value]) == -CompareTo)))
            {
                int min;
                if (rightChild == null) min = leftChild.Value;
                else if (elements[leftChild.Value].CompareTo(elements[rightChild.Value]) == CompareTo) min = leftChild.Value;
                else min = rightChild.Value;

                Swap(min, current);

                current = min;
                rightChild = (current + 1) * 2;
                leftChild = (current + 1) * 2 - 1;
                if (leftChild >= elements.Count) leftChild = null;
                if (rightChild >= elements.Count) rightChild = null;
            }
        }

        private void BubbleUp()
        {
            if (elements.Count < 2) return;

            var current = elements.Count - 1;
            var parent = (current + 1) / 2 - 1;

            while (parent >= 0 && elements[current].CompareTo(elements[parent]) == CompareTo)
            {
                Swap(parent, current);
                current = parent;
                parent = (current + 1) / 2 - 1;
            }
        }

        private void Swap(int i, int j)
        {
            var temporary = elements[i];
            elements[i] = elements[j];
            elements[j] = temporary;
        }
    }

    internal class MedianHeap
    {
        private readonly MaxHeap leftHeap;
        private readonly MinHeap rightHeap;

        public MedianHeap()
        {
            leftHeap = new MaxHeap();
            rightHeap = new MinHeap();
        }

        public long Pop()
        {
            if (rightHeap.Count == leftHeap.Count)
            {
                return leftHeap.Pop();
            }
            else if (leftHeap.Count > rightHeap.Count)
            {
                return leftHeap.Pop();
            }
            else
            {
                return rightHeap.Pop();
            }
        }

        public void Push(long item)
        {
            if (rightHeap.Count == 0 && leftHeap.Count < 2)
            {
                leftHeap.Push(item);
            }
            else
            {
                if (item < rightHeap.Get())
                {
                    leftHeap.Push(item);
                }
                else
                {
                    rightHeap.Push(item);
                }
            }

            Rebalance();
        }

        public void Rebalance()
        {
            if (leftHeap.Count - rightHeap.Count > 1)
            {
                rightHeap.Push(leftHeap.Pop());
            }
            else if (rightHeap.Count - leftHeap.Count > 1)
            {
                leftHeap.Push(rightHeap.Pop());
            }
        }
    }

    internal class MinHeap : Heap
    {
        public MinHeap()
        {
            CompareTo = -1;
        }
    }

    internal class MaxHeap : Heap
    {
        public MaxHeap()
        {
            CompareTo = 1;
        }
    }
}
