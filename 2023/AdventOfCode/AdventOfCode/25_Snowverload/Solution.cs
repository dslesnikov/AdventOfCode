namespace AdventOfCode._25_Snowverload;

public class Solution(IReadOnlyDictionary<string, IReadOnlySet<string>> graph) : ISolution<Solution>
{
    public static int Day => 25;

    public static Solution Parse(string s)
    {
        var input = s.Split('\n')
            .Select(line =>
            {
                var parts = line.Split(": ");
                var from = parts[0];
                var to = parts[1].Split(' ');
                return (From: from, To: to);
            });
        var graph = new Dictionary<string, HashSet<string>>();
        foreach (var line in input)
        {
            if (!graph.TryGetValue(line.From, out var set))
            {
                graph[line.From] = set = new HashSet<string>();
            }
            set.UnionWith(line.To);
            foreach (var node in line.To)
            {
                if (!graph.TryGetValue(node, out var adjacency))
                {
                    graph[node] = adjacency = new HashSet<string>();
                }
                adjacency.Add(line.From);
            }
        }
        var result = graph.ToDictionary(x => x.Key, x => (IReadOnlySet<string>)x.Value);
        return new Solution(result);
    }

    public string Part1()
    {
        var popularEdges = new Dictionary<(string, string), int>();
        var visited = new HashSet<string>();
        var queue = new Queue<string>();
        var sourceNodes = new Dictionary<string, string>();
        foreach (var from in graph.Keys)
        {
            visited.Clear();
            queue.Clear();
            sourceNodes.Clear();
            queue.Enqueue(from);
            visited.Add(from);
            while (queue.TryDequeue(out var currentNode))
            {
                foreach (var neighbor in graph[currentNode])
                {
                    if (!visited.Add(neighbor))
                    {
                        continue;
                    }
                    sourceNodes[neighbor] = currentNode;
                    queue.Enqueue(neighbor);
                }
            }
            foreach (var end in visited)
            {
                var current = end;
                while (sourceNodes.TryGetValue(current, out var sourceNode))
                {
                    var edge = StringComparer.Ordinal.Compare(current, sourceNode) <= 0
                        ? (current, sourceNode)
                        : (sourceNode, current);
                    popularEdges.TryAdd(edge, 0);
                    popularEdges[edge]++;
                    current = sourceNode;
                }
            }
        }
        var edgesToIgnore = popularEdges
            .OrderByDescending(x => x.Value)
            .Take(3)
            .SelectMany(x => new[]
            {
                (x.Key.Item1, x.Key.Item2),
                (x.Key.Item2, x.Key.Item1)
            })
            .ToHashSet();
        var start = graph.Keys.First();
        visited.Clear();
        queue.Clear();
        queue.Enqueue(start);
        visited.Add(start);
        while (queue.TryDequeue(out var currentNode))
        {
            foreach (var neighbor in graph[currentNode])
            {
                if (!visited.Add(neighbor) ||
                    edgesToIgnore.Contains((currentNode, neighbor)))
                {
                    continue;
                }
                queue.Enqueue(neighbor);
            }
        }
        var visitedCount = visited.Count;
        var total = graph.Count;
        var remaining = total - visitedCount;
        return (visitedCount * remaining).ToString();
    }

    public string Part2()
    {
        return "Merry Christmas!";
    }
}
