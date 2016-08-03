using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DijkstrasShortestPath
{
    internal class Program
    {
        private const int VerticesCount = 200;
        private static readonly int[] Output = new int[10] { 7, 37, 59, 82, 99, 115, 133, 165, 188, 197 };

        private static void Main()
        {
            var edges = ReadData(@"..\..\dijkstraData.txt");

            var weights = Dijkstra(edges);

            foreach (var weight in Output)
            {
                Console.WriteLine(weights[weight - 1]);
            }

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        private static int[] Dijkstra(List<Tuple<int, int>>[] edges)
        {
            var weights = new int[edges.Length];
            var processed = new List<int>() { 1 };

            while (processed.Count < edges.Length)
            {
                Tuple<int, int> minEdge = null;
                int minWeight = int.MaxValue, minVertex = 1;

                for (var i = 1; i <= edges.Length; i++)
                {
                    if (!processed.Contains(i)) continue; // edge starts not in processed part
                    foreach (var edge in edges[i - 1])
                    {
                        if (processed.Contains(edge.Item1)) continue; // edge goes to processed part
                        if (weights[i - 1] + edge.Item2 < minWeight) // is edge with min weight so far
                        {
                            minVertex = i;
                            minEdge = edge;
                            minWeight = weights[i - 1] + edge.Item2;
                        }
                    }
                }

                processed.Add(minEdge.Item1);
                edges[minVertex - 1].Remove(minEdge); // edge is processed
                weights[minEdge.Item1 - 1] = weights[minVertex - 1] + minEdge.Item2;
            }

            return weights;
        }

        private static List<Tuple<int, int>>[] ReadData(string path)
        {
            var edges = new List<Tuple<int, int>>[VerticesCount];
            for (int i = 0; i < VerticesCount; i++)
            {
                edges[i] = new List<Tuple<int, int>>(); // i - edge goes from, Tuple<int, int> - <edge goes to, weight>
            }

            foreach (var line in File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)))
            {
                var tuples = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                var vertex = int.Parse(tuples[0]);

                for (var i = 1; i < tuples.Length; i++)
                {
                    var edge = tuples[i].Split(new[] { ',' });
                    edges[vertex - 1].Add(new Tuple<int, int>(int.Parse(edge[0]), int.Parse(edge[1])));
                }
            }

            return edges;
        }
    }
}
