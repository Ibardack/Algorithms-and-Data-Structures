using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Principal;

namespace Dijkstra
{
    internal class InfluenceLogic
    {
        // === Braedth-First Search (Unweighted) ===
        private static Dictionary<string, int> BFS(string start)
        {
            var dist = GraphLoader.GraphData.Keys.ToDictionary(x => x, x => int.MaxValue);
            var q = new Queue<string>();

            dist[start] = 0;
            q.Enqueue(start);

            while (q.Count > 0)
            {
                string current = q.Dequeue();

                if (!GraphLoader.GraphData.ContainsKey(current)) continue;

                foreach (var (next, _) in GraphLoader.GraphData[current])
                {

                    if (dist[next] == int.MaxValue)
                    {
                        dist[next] = dist[current] + 1;
                        q.Enqueue(next);
                    }

                }
            }
            return dist;
        }

        // === Dijkstra (Weighted) ===

        private static Dictionary<string, double> Dijkstra(string start)

        {
            var dist = GraphLoader.GraphData.Keys.ToDictionary(x => x, x => double.PositiveInfinity);
            var visited = GraphLoader.GraphData.Keys.ToDictionary(x => x, x => false);

            dist[start] = 0;

            for (int i = 0; i < GraphLoader.NodeCount; i++)
            {
                string best = null;
                double bestVal = double.PositiveInfinity;

                foreach (var node in dist.Keys)
                {
                    if (!visited[node] && dist[node] < bestVal)
                    {
                        bestVal = dist[node];
                        best = node;
                    }
                }
                if (best == null) break;

                visited[best] = true;

                foreach (var (next, weight) in GraphLoader.GraphData[best])
                {
                    double newDist = dist[best] + weight;
                    if (newDist < dist[next])
                        dist[next] = newDist;
                }
            }
            return dist;
        }

        // === Unweighted Influence ===

        public static Dictionary<string, double> CalculateUnweighted()
        {
            var scores = new Dictionary<string, double>();
            int n = GraphLoader.NodeCount;

            foreach (string node in GraphLoader.GraphData.Keys)
            {
                var dist = BFS(node);
                int sum = dist.Values.Where(x => x != int.MaxValue).Sum();

                double score = sum == 0 ? 0 : (double)(n - 1) / sum;
                scores[node] = score;
            }
            return scores;
        }

        // === Wighted Influence ===

        public static Dictionary<string, double> CalculateWeighted()
        {

            var scores = new Dictionary<string, double>();
            int n = GraphLoader.NodeCount;

            foreach (string node in GraphLoader.GraphData.Keys)
            {
                var dist = Dijkstra(node);
                double sum = dist.Values.Sum();

                double score = sum == 0 ? 0 : (double)(n - 1) / sum;
                scores[node] = score;
            }

            return scores;
        }

        // === Print Results ===

        public static void PrintBestWorst(Dictionary<string, double> scores)
        {
            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("No nodes to calculate influence.");
                return;
            }

            var sorted = scores.OrderByDescending(x => x.Value).ToList();
            var best = sorted.First();
            var worst = sorted.Last();

            Console.WriteLine();
            Console.WriteLine($"Best Influence : {best.Key} (score {best.Value:F3})");
            Console.WriteLine($"Worst Influence : {worst.Key} (score {worst.Value:F3})");
        }


        public static void PrintAllScores(Dictionary<string, double> scores)
        {
            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("No nodes to calculate influence.");
                return;
            }

            Console.WriteLine("\nAll Influence Scores:");
            foreach (var kvp in scores.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value:F3}");
            }
        }


    }
}