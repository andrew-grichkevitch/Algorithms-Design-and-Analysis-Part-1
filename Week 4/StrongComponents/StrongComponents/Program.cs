using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace StrongComponents
{
    internal class Programm
    {
        private const int VerticesCount = 875714;
        private const int MaxStackSize = 10000000;

        private static void Main()
        {
            new Thread(new ThreadStart(RunSCCs), MaxStackSize).Start();
        }

        private static void RunSCCs()
        {
            var edges = ReadData(@"..\..\SCC.txt");

            // G_rev = G_edges with all arcs reversed
            var rev = new List<int>[VerticesCount];
            for (int i = 0; i < VerticesCount; i++)
            {
                rev[i] = new List<int>();
            }

            for (int i = 0; i < VerticesCount; i++)
            {
                foreach (var edge in edges[i])
                {
                    rev[edge - 1].Add(i + 1);
                }
            }

            // compute “magical ordering” of nodes
            var order = new List<int>();
            var vertices = new bool[VerticesCount];

            for (var i = vertices.Length; i > 0; i--)
            {
                if (!vertices[i - 1]) DFS(vertices, rev, i, order);
            }

            // discover the SCCs one-­by­‐one
            var components = new List<int>();
            vertices = new bool[VerticesCount];

            for (var i = vertices.Length; i > 0; i--)
            {
                int count = 0;
                if (!vertices[order[i - 1] - 1])
                {
                    DFS(vertices, edges, order[i - 1], ref count);
                    components.Add(count);
                }
            }

            // get top 5 components
            components.Sort(new Comparison<int>((i1, i2) => i2.CompareTo(i1)));

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(components.Count > i ? components[i] : 0);
            }

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        private static List<int>[] ReadData(string path)
        {
            var edges = new List<int>[VerticesCount];
            for (int i = 0; i < VerticesCount; i++)
            {
                edges[i] = new List<int>();
            }

            foreach (var line in File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)))
            {
                var edge = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                edges[edge[0] - 1].Add(edge[1]);
            }

            return edges;
        }

        private static void DFS(bool[] vertices, List<int>[] edges, int vertex, ref int count)
        {
            count++;
            vertices[vertex - 1] = true;

            foreach (var edge in edges[vertex - 1])
            {
                if (!vertices[edge - 1]) DFS(vertices, edges, edge, ref count);
            }
        }

        private static void DFS(bool[] vertices, List<int>[] edges, int vertex, List<int> order)
        {
            vertices[vertex - 1] = true;

            foreach (var edge in edges[vertex - 1])
            {
                if (!vertices[edge - 1]) DFS(vertices, edges, edge, order);
            }

            order.Add(vertex);
        }
    }
}
