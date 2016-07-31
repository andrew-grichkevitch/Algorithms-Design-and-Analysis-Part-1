using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MinCut
{
    internal class Program
    {
        private static void Main()
        {
            List<int> vertices;
            List<Tuple<int, int>> edges;
            ReadData(@"..\..\kargerMinCut.txt", '\t', out vertices, out edges);

            var minCut = int.MaxValue;
            for (var i = 0; i < vertices.Count; i++)
            {
                var cut = MinCut(vertices.ToList(), edges.ToList());
                if (cut < minCut) minCut = cut;
            }

            Console.WriteLine(minCut);
            Console.ReadLine();
        }

        private static int MinCut(List<int> vertices, List<Tuple<int, int>> edges)
        {
            var random = new Random();
            while (vertices.Count > 2)
            {
                var edge = edges[random.Next(edges.Count - 1)];
                vertices.Remove(edge.Item1); // merge two vertices into a single vertex - edge.Item2

                for (var i = 0; i < edges.Count; i++)
                {
                    if (edges[i].Item1 == edge.Item1)
                    {
                        edges[i] = new Tuple<int, int>(edge.Item2, edges[i].Item2);
                    }
                    else if (edges[i].Item2 == edge.Item1)
                    {
                        edges[i] = new Tuple<int, int>(edges[i].Item1, edge.Item2);
                    }
                }

                edges.RemoveAll(e => e.Item1 == e.Item2); // remove self-loops
            }

            return edges.Count;
        }

        private static void ReadData(string path, char split, out List<int> vertices, out List<Tuple<int, int>> edges)
        {
            vertices = new List<int>();
            edges = new List<Tuple<int, int>>();

            foreach (var line in File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)))
            {
                var numbers = line.Split(new[] {split}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                vertices.Add(numbers[0]);

                for (var i = 1; i < numbers.Length; i++)
                {
                    if (!edges.Any(e => e.Item1 == numbers[i] && e.Item2 == numbers[0]))
                    {
                        edges.Add(new Tuple<int, int>(numbers[0], numbers[i]));
                    }
                }
            }
        }
    }
}
