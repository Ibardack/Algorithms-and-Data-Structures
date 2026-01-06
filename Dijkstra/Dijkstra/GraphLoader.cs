using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Dijkstra
{
    internal class GraphLoader
    {
        public static string Path { get; set; }

        public static Dictionary<string, List<(string neighbour, double weight)>> GraphData
            = new Dictionary<string, List<(string neighbour, double weight)>>();

        public static int NodeCount => GraphData.Count;

        // === Unweighted ===
        public static void LoadUnweighted()
        {
            EnsurePath();
            GraphData.Clear();

            foreach (string line in File.ReadLines(Path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length < 2) continue;

                string a = parts[0].Trim();
                string b = parts[1].Trim();

                AddEdge(a, b, 1);
                AddEdge(b, a, 1);
            }
        }

        // === Weighted ===
        public static void LoadWeighted()
        {
            EnsurePath();
            GraphData.Clear();

            bool firstLine = true;

            foreach (string line in File.ReadLines(Path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length < 3) continue;

                string a = parts[0].Trim();
                string b = parts[1].Trim();
                string wText = parts[2].Trim();

                if (firstLine)
                {
                    firstLine = false;
                    if (!double.TryParse(wText, out _)) continue;
                }

                if (!double.TryParse(wText, out double w)) continue;

                AddEdge(a, b, w);
                AddEdge(b, a, w);
            }
        }

        private static void AddEdge(string from, string to, double weight)
        {
            if (!GraphData.ContainsKey(from))
                GraphData[from] = new List<(string, double)>();

            if (!GraphData.ContainsKey(to))
                GraphData[to] = new List<(string, double)>();

            GraphData[from].Add((to, weight));
        }

        private static void EnsurePath()
        {
            if (string.IsNullOrWhiteSpace(Path))
                throw new InvalidOperationException("Graph path not set.");

            if (!File.Exists(Path))
                throw new FileNotFoundException("Graph file not found.", Path);
        }
    }
}

