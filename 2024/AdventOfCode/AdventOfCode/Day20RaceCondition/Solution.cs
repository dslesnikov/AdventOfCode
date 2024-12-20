namespace AdventOfCode.Day20RaceCondition;

public class Solution : IFromLines<Solution, IReadOnlyList<Tile>>
{
    public static int Day => 20;

    private readonly IReadOnlyList<IReadOnlyList<Tile>> _map;
    private readonly Point _start;
    private readonly Point _end;

    private Solution(IReadOnlyList<IReadOnlyList<Tile>> map)
    {
        _map = map;
        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == Tile.Start)
                {
                    _start = new Point(x, y);
                }
                else if (map[y][x] == Tile.End)
                {
                    _end = new Point(x, y);
                }
            }
        }
    }

    private Tile this[Point point] => _map[point.Y][point.X];

    public string SolvePartOne()
    {
        return CountGoodCheats(2).ToString();
    }

    public string SolvePartTwo()
    {
        return CountGoodCheats(20).ToString();
    }

    private int CountGoodCheats(int maxCheatingLength)
    {
        var regularLength = CalculatePathLengths();

        var visited = new HashSet<Point>();
        var queue = new Queue<(Point Point, int Length)>();
        queue.Enqueue((_start, 0));
        visited.Add(_start);

        Span<Point> neighbors = stackalloc Point[4];
        Span<(Point End, int Length)> cheats = new (Point, int)[1000];
        var allCheats = new Dictionary<(Point Start, Point End), int>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Point == _end)
            {
                break;
            }
            var cheatLength = GetCheats(current.Point, maxCheatingLength, cheats);
            for (var i = 0; i < cheatLength; i++)
            {
                if (!visited.Contains(cheats[i].End))
                {
                    var normalLength = regularLength[cheats[i].End];
                    var cheatingLength = current.Length + cheats[i].Length;
                    var saved = normalLength - cheatingLength;
                    if (saved > 0 &&
                        (!allCheats.TryGetValue((current.Point, cheats[i].End), out var existing) ||
                         saved > existing))
                    {
                        allCheats[(current.Point, cheats[i].End)] = saved;
                    }
                }
            }
            var length = GetNeighbors(current.Point, neighbors);
            for (var i = 0; i < length; i++)
            {
                if (this[neighbors[i]] != Tile.Wall && visited.Add(neighbors[i]))
                {
                    queue.Enqueue((neighbors[i], current.Length + 1));
                }
            }
        }

        var goodCheats = allCheats.Count(x => x.Value >= 100);
        return goodCheats;
    }

    private Dictionary<Point, int> CalculatePathLengths()
    {
        var queue = new Queue<(Point Point, int Length)>();
        queue.Enqueue((_start, 0));
        var visited = new Dictionary<Point, int> { [_start] = 0 };

        Span<Point> neighbors = stackalloc Point[4];
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Point == _end)
            {
                break;
            }
            var length = GetNeighbors(current.Point, neighbors);
            for (var i = 0; i < length; i++)
            {
                if (this[neighbors[i]] != Tile.Wall && visited.TryAdd(neighbors[i], current.Length + 1))
                {
                    queue.Enqueue((neighbors[i], current.Length + 1));
                }
            }
        }

        return visited;
    }

    private int GetCheats(Point start, int maxLength, Span<(Point End, int Length)> cheats)
    {
        var visited = new HashSet<Point>();
        var queue = new Queue<(Point Point, int Length)>();
        queue.Enqueue((start, 0));
        visited.Add(start);

        Span<Point> neighbors = stackalloc Point[4];
        var resultLength = 0;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Length > maxLength)
            {
                continue;
            }
            if (current.Length > 0 && this[current.Point] != Tile.Wall)
            {
                cheats[resultLength++] = (current.Point, current.Length);
            }
            var length = GetNeighbors(current.Point, neighbors);
            for (var i = 0; i < length; i++)
            {
                if (visited.Add(neighbors[i]))
                {
                    queue.Enqueue((neighbors[i], current.Length + 1));
                }
            }
        }

        return resultLength;
    }

    private int GetNeighbors(Point current, Span<Point> neighbors)
    {
        var length = 0;
        if (current.X > 0)
        {
            neighbors[length++] = current with { X = current.X - 1 };
        }
        if (current.X < _map[current.Y].Count - 1)
        {
            neighbors[length++] = current with { X = current.X + 1 };
        }
        if (current.Y > 0)
        {
            neighbors[length++] = current with { Y = current.Y - 1 };
        }
        if (current.Y < _map.Count - 1)
        {
            neighbors[length++] = current with { Y = current.Y + 1 };
        }

        return length;
    }

    public static IReadOnlyList<Tile> ParseLine(ReadOnlySpan<char> line)
    {
        var tiles = new Tile[line.Length];
        for (var i = 0; i < line.Length; i++)
        {
            tiles[i] = line[i] switch
            {
                '.' => Tile.Empty,
                '#' => Tile.Wall,
                'S' => Tile.Start,
                'E' => Tile.End,
                _ => throw new InvalidOperationException()
            };
        }

        return tiles;
    }

    public static Solution FromParsed(IReadOnlyList<IReadOnlyList<Tile>> entries)
    {
        return new Solution(entries);
    }
}