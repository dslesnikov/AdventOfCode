namespace AdventOfCode._23_ALongWalk;

public class Solution(IReadOnlyList<IReadOnlyList<char>> map) : ISolution<Solution>
{
    public static int Day => 23;

    public static Solution Parse(string s)
    {
        var map = s.Split('\n')
            .Select(line => line.ToCharArray())
            .ToList();
        return new Solution(map);
    }

    public string Part1()
    {
        var start = FindStart();
        var end = FindEnd();
        var queue = new Queue<(int Row, int Col, int Distance, HashSet<(int Row, int Col)> Visited)>();
        queue.Enqueue((start.Row, start.Col, 0, [start]));
        Span<(int Row, int Col)> neighbors = stackalloc (int Row, int Col)[4];
        var maxDistance = 0;
        while (queue.TryDequeue(out var current))
        {
            if (current.Row == end.Row &&
                current.Col == end.Col)
            {
                maxDistance = Math.Max(maxDistance, current.Distance);
                continue;
            }
            var count = GetNeighbors(current.Row, current.Col, neighbors, true);
            var first = true;
            for (var i = 0; i < count; i++)
            {
                var neighbor = neighbors[i];
                if (current.Visited.Contains(neighbor))
                {
                    continue;
                }
                var visited = first
                    ? current.Visited
                    : [..current.Visited];
                first = false;
                visited.Add(neighbor);
                queue.Enqueue((neighbor.Row, neighbor.Col, current.Distance + 1, visited));
            }
        }
        return maxDistance.ToString();
    }

    public string Part2()
    {
        var start = FindStart();
        var end = FindEnd();
        var graph = BuildGraph(start, end);
        var queue = new Queue<(int Row, int Col, int Distance, HashSet<(int Row, int Col)> Visited)>();
        var maxDistance = 0;
        queue.Enqueue((start.Row, start.Col, 0, [start]));
        while (queue.TryDequeue(out var current))
        {
            if (current.Row == end.Row &&
                current.Col == end.Col)
            {
                if (current.Distance > maxDistance)
                {
                    maxDistance = current.Distance;
                }
                continue;
            }
            if (!graph.TryGetValue((current.Row, current.Col), out var adjacency))
            {
                continue;
            }
            foreach (var (row, col, distance) in adjacency)
            {
                if (current.Visited.Contains((row, col)))
                {
                    continue;
                }
                var visited = new HashSet<(int Row, int Col)>(current.Visited)
                {
                    (row, col)
                };
                queue.Enqueue((row, col, current.Distance + distance, visited));
            }
        }
        return maxDistance.ToString();
    }

    private int GetNeighbors(int row, int col,
        Span<(int Row, int Col)> neighbors,
        bool slippery)
    {
        if (slippery)
        {
            switch (map[row][col])
            {
                case '>':
                    neighbors[0] = (row, col + 1);
                    return 1;
                case '<':
                    neighbors[0] = (row, col - 1);
                    return 1;
                case '^':
                    neighbors[0] = (row - 1, col);
                    return 1;
                case 'v':
                    neighbors[0] = (row + 1, col);
                    return 1;
            }
        }
        var count = 0;
        if (row > 0 && map[row - 1][col] != '#')
        {
            neighbors[count++] = (row - 1, col);
        }
        if (row < map.Count - 1 && map[row + 1][col] != '#')
        {
            neighbors[count++] = (row + 1, col);
        }
        if (col > 0 && map[row][col - 1] != '#')
        {
            neighbors[count++] = (row, col - 1);
        }
        if (col < map[0].Count - 1 && map[row][col + 1] != '#')
        {
            neighbors[count++] = (row, col + 1);
        }
        return count;
    }

    private Dictionary<(int Row, int Col), HashSet<(int Row, int Col, int Distance)>> BuildGraph(
        (int Row, int Col) start,
        (int Row, int Col) end)
    {
        var graph = new Dictionary<(int Row, int Col), HashSet<(int Row, int Col, int Distance)>>
        {
            { start, [] },
            { end, [] }
        };
        for (var row = 1; row < map.Count - 1; row++)
        {
            for (var col = 1; col < map[0].Count - 1; col++)
            {
                if (map[row][col] != '.')
                {
                    continue;
                }
                var passableNeighbors = 0;
                if (map[row - 1][col] != '#')
                {
                    passableNeighbors++;
                }
                if (map[row + 1][col] != '#')
                {
                    passableNeighbors++;
                }
                if (map[row][col - 1] != '#')
                {
                    passableNeighbors++;
                }
                if (map[row][col + 1] != '#')
                {
                    passableNeighbors++;
                }
                if (passableNeighbors > 2)
                {
                    graph[(row, col)] = [];
                }
            }
        }
        var queue = new Queue<(int Row, int Col, int Distance)>();
        var visited = new HashSet<(int Row, int Col)>();
        Span<(int Row, int Col)> neighbors = stackalloc (int Row, int Col)[4];
        foreach (var (node, adjacency) in graph)
        {
            queue.Clear();
            visited.Clear();
            queue.Enqueue((node.Row, node.Col, 0));
            visited.Add(node);
            while (queue.TryDequeue(out var current))
            {
                if (node != (current.Row, current.Col) &&
                    graph.ContainsKey((current.Row, current.Col)))
                {
                    adjacency.Add((current.Row, current.Col, current.Distance));
                    continue;
                }
                var count = GetNeighbors(current.Row, current.Col, neighbors, false);
                for (var i = 0; i < count; i++)
                {
                    var neighbor = neighbors[i];
                    if (!visited.Add(neighbor))
                    {
                        continue;
                    }
                    queue.Enqueue((neighbor.Row, neighbor.Col, current.Distance + 1));
                }
            }
        }
        return graph;
    }

    private (int Row, int Col) FindStart()
    {
        for (var col = 0; col < map[0].Count; col++)
        {
            if (map[0][col] == '.')
            {
                return (0, col);
            }
        }
        return (-1, -1);
    }

    private (int Row, int Col) FindEnd()
    {
        for (var col = 0; col < map[0].Count; col++)
        {
            if (map[^1][col] == '.')
            {
                return (map.Count - 1, col);
            }
        }
        return (-1, -1);
    }
}