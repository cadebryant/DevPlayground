using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using workspace.lib;

namespace workspace
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = new PriorityQueue(6);
            queue.AddNew(3);
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.AddNew(4);
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.AddNew(2);
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.AddNew(11);
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.AddNew(1);
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.AddNew(9);
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.GetMin();
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.GetMin();
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.GetMin();
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.GetMin();
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.GetMin();
            queue.DisplayElements();
            Console.WriteLine("----------------------");
            queue.GetMin();
            queue.DisplayElements();
            Console.WriteLine("----------------------");            

            // var graph = new Graph(File.ReadAllText("data/DijkstraGraph1.json"));
            // graph.Dijkstra("S");
            // graph.DistanceTable.ToList().ForEach(dt => Console.WriteLine((dt.Key, dt.Value.Item1, dt.Value.Item2)));
        }
    }

    public class Graph
    {
        public int Size { get; private set; }
        public AdjacencyList AdjacencyList { get; set; }
        public Dictionary<string, (int, string)> DistanceTable { get; private set; }

        public Graph(string graphJson)
        {
            AdjacencyList = JsonConvert.DeserializeObject<AdjacencyList>(graphJson);
            AdjacencyList.Unexplored = new HashSet<string>(AdjacencyList.Keys);
            DistanceTable = new Dictionary<string, (int, string)>();
            foreach (var label in AdjacencyList.Unexplored)
            {
                DistanceTable[label] = (int.MaxValue / 2, null);
            }
        }

        public void Dijkstra(string startVertexLabel)
        {
            var currVertexLabel = startVertexLabel;
            DistanceTable[currVertexLabel] = (0, null);
            while (AdjacencyList.Unexplored.Count > 0)
            {
                if (currVertexLabel == null) continue;
                foreach (var neighbor in AdjacencyList.GetNeighbors(currVertexLabel))
                {
                    if (!AdjacencyList.Unexplored.Contains(neighbor.Item1)) continue;
                    var newDistance = neighbor.Item2 + DistanceTable[currVertexLabel].Item1;
                    if (newDistance < DistanceTable[neighbor.Item1].Item1)
                    {
                        DistanceTable[neighbor.Item1] = (newDistance, currVertexLabel);
                    }
                }
                AdjacencyList.Unexplored.Remove(currVertexLabel);
                currVertexLabel = GetEdgeWithBestScore();
            }
        }

        public string GetEdgeWithBestScore()
        {
            return DistanceTable
                .Where(v => AdjacencyList.Unexplored.Contains(v.Key) && !AdjacencyList.Unexplored.Contains(v.Value.Item2))
                .Where(v => v.Value.Item1 ==
                       DistanceTable
                       .Where(v => AdjacencyList.Unexplored.Contains(v.Key) && !AdjacencyList.Unexplored.Contains(v.Value.Item2))
                       .Min(v => v.Value.Item1))
                .Select(e => e.Key)
                .FirstOrDefault();
        }
    }

    public class Vertex
    {
        public string Label { get; set; }
        public int Distance { get; set; }
        public List<Edge> Edges { get; set; }
    }

    public class Edge
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Length { get; set; }
    }

    public class AdjacencyList : Dictionary<string, Vertex>
    {
        public HashSet<string> Unexplored { get; set; }
        public AdjacencyList()
        {
            Unexplored = new HashSet<string>();
        }
        public int GetDistance(string vertexLabel)
        {
            return this[vertexLabel].Distance;
        }

        public void SetDistance(string vertexLabel, int distance)
        {
            if (vertexLabel == null) return;
            this[vertexLabel].Distance = distance;
        }

        public List<Edge> GetEdges(string vertexLabel)
        {
            return this[vertexLabel].Edges;
        }

        public List<(string, int)> GetNeighbors(string vertexLabel)
        {
            return GetEdges(vertexLabel).Select(e => (e.To, e.Length)).ToList();
        }
    }
}
