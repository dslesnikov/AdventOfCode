namespace AdventOfCode.Day18RamRun;

public readonly record struct Point(int X, int Y);

public class Solution : IFromLines<Solution, Point>
{
    public static int Day => 18;

    private readonly IReadOnlyList<Point> _points;

    private Solution(IReadOnlyList<Point> points)
    {
        _points = points;
    }

    public string SolvePartOne()
    {
        var result = Traverse(1024);
        return result?.ToString() ?? "No path found";
    }

    public string SolvePartTwo()
    {
        for (var corruptedLength = 1024; corruptedLength <= _points.Count; corruptedLength++)
        {
            var length = Traverse(corruptedLength);
            if (length is null)
            {
                var point = _points[corruptedLength - 1];
                return $"{point.X},{point.Y}";
            }
        }

        throw new InvalidOperationException();
    }

    private int? Traverse(int corruptedLength)
    {
        const int width = 71;
        const int height = 71;
        var start = new Point(0, 0);
        var end = new Point(width - 1, height - 1);
        var corrupted = _points.Take(corruptedLength).ToHashSet();
        var visited = new HashSet<Point> { start };
        var queue = new Queue<(Point Point, int Length)>();
        queue.Enqueue((start, 0));
        Span<Point> neighbors = stackalloc Point[4];
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Point == end)
            {
                return current.Length;
            }
            var neighborsLength = GetNeighbors(neighbors, current.Point, width, height);
            for (var i = 0; i < neighborsLength; i++)
            {
                var neighbor = neighbors[i];
                if (corrupted.Contains(neighbor) || !visited.Add(neighbor))
                {
                    continue;
                }
                queue.Enqueue((neighbor, current.Length + 1));
            }
        }

        return null;
    }

    private int GetNeighbors(Span<Point> neighbors, Point current, int width, int height)
    {
        var length = 0;
        if (current.X > 0)
        {
            neighbors[length++] = current with { X = current.X - 1 };
        }
        if (current.X < width - 1)
        {
            neighbors[length++] = current with { X = current.X + 1 };
        }
        if (current.Y > 0)
        {
            neighbors[length++] = current with { Y = current.Y - 1 };
        }
        if (current.Y < height - 1)
        {
            neighbors[length++] = current with { Y = current.Y + 1 };
        }
        return length;
    }

    public static Point ParseLine(ReadOnlySpan<char> line)
    {
        var split = line.Split(',');
        split.MoveNext();
        var x = int.Parse(line[split.Current.Start..split.Current.End]);
        split.MoveNext();
        var y = int.Parse(line[split.Current.Start..split.Current.End]);
        return new Point(x, y);
    }

    public static Solution FromParsed(IReadOnlyList<Point> entries)
    {
        return new Solution(entries);
    }
}