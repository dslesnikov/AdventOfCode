using System.Collections.Immutable;

namespace AdventOfCode.Day21KeypadConundrum;

public class Solution : IFromLines<Solution, string>
{
    public static int Day => 21;

    private readonly IReadOnlyList<string> _codes;
    private readonly ImmutableDictionary<(char From, char To),ImmutableArray<string>> _numericPaths;
    private readonly ImmutableDictionary<(char From, char To),ImmutableArray<string>> _directionalPaths;

    private Solution(IReadOnlyList<string> codes)
    {
        _codes = codes;
        _numericPaths = BuildNumericPaths();
        _directionalPaths = BuildDirectionalPaths();
    }

    public string SolvePartOne()
    {
        var result = CalculateComplexities(2);
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var result = CalculateComplexities(25);
        return result.ToString();
    }

    private long CalculateComplexities(int maxLevel)
    {
        var result = 0L;
        var cache = new Dictionary<(char From, char To, int Level), long>();
        foreach (var code in _codes)
        {
            var current = $"A{code}";
            var minLength = 0L;
            for (var i = 0; i < current.Length - 1; i++)
            {
                minLength += CalculateMinLength(current[i], current[i + 1], maxLevel, cache);
            }
            result += minLength * int.Parse(code.AsSpan(0, code.Length - 1));
        }
        return result;
    }

    private long CalculateMinLength(char from, char to, int level, Dictionary<(char, char, int), long> cache)
    {
        if (cache.TryGetValue((from, to, level), out var result))
        {
            return result;
        }
        var paths = from is '<' or '>' or 'v' or '^' || to is '<' or '>' or 'v' or '^'
            ? _directionalPaths[(from, to)]
            : _numericPaths[(from, to)];
        if (level == 0)
        {
            result = paths.Min(p => p.Length);
            cache[(from, to, level)] = result;
            return result;
        }
        var min = long.MaxValue;
        foreach (var path in paths)
        {
            var length = 0L;
            for (var i = 0; i < path.Length; i++)
            {
                length += CalculateMinLength(i == 0 ? 'A' : path[i - 1], path[i], level - 1, cache);
            }
            min = Math.Min(min, length);
        }
        cache[(from, to, level)] = min;
        return min;
    }

    private static ImmutableDictionary<(char From, char To), ImmutableArray<string>> BuildDirectionalPaths()
    {
        var positionBuilder = ImmutableDictionary.CreateBuilder<char, (int X, int Y)>();
        positionBuilder.Add('^', (1, 0));
        positionBuilder.Add('A', (2, 0));
        positionBuilder.Add('<', (0, 1));
        positionBuilder.Add('v', (1, 1));
        positionBuilder.Add('>', (2, 1));
        var positions = positionBuilder.ToImmutable();
        var result = new Dictionary<(char From, char To), ImmutableArray<string>>();
        foreach (var from in positions.Keys)
        {
            foreach (var to in positions.Keys)
            {
                if (from == to)
                {
                    result[(from, to)] = ["A"];
                    continue;
                }
                result[(from, to)] = BuildPaths(from, to, positions, (0, 0));
            }
        }
        return result.ToImmutableDictionary();
    }

    private static ImmutableDictionary<(char From, char To), ImmutableArray<string>> BuildNumericPaths()
    {
        var positionBuilder = ImmutableDictionary.CreateBuilder<char, (int X, int Y)>();
        positionBuilder.Add('7', (0, 0));
        positionBuilder.Add('8', (1, 0));
        positionBuilder.Add('9', (2, 0));
        positionBuilder.Add('4', (0, 1));
        positionBuilder.Add('5', (1, 1));
        positionBuilder.Add('6', (2, 1));
        positionBuilder.Add('1', (0, 2));
        positionBuilder.Add('2', (1, 2));
        positionBuilder.Add('3', (2, 2));
        positionBuilder.Add('0', (1, 3));
        positionBuilder.Add('A', (2, 3));
        var positions = positionBuilder.ToImmutable();
        var result = new Dictionary<(char From, char To), ImmutableArray<string>>();
        foreach (var from in positions.Keys)
        {
            foreach (var to in positions.Keys)
            {
                if (from == to)
                {
                    result[(from, to)] = ["A"];
                    continue;
                }
                result[(from, to)] = BuildPaths(from, to, positions, (0, 3));
            }
        }
        return result.ToImmutableDictionary();
    }

    private static ImmutableArray<string> BuildPaths(
        char from,
        char to,
        ImmutableDictionary<char, (int X, int Y)> positions,
        (int X, int Y) forbidden)
    {
        var start = positions[from];
        var end = positions[to];
        var queue = new Queue<((int X, int Y) Point, List<char> Path, HashSet<(int, int)> Visited)>();
        queue.Enqueue((start, [], [start]));
        var paths = new List<string>();
        var minLength = int.MaxValue;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Point == end)
            {
                if (current.Path.Count < minLength)
                {
                    paths.Clear();
                }
                minLength = Math.Min(minLength, current.Path.Count);
                if (current.Path.Count == minLength)
                {
                    current.Path.Add('A');
                    paths.Add(new string(current.Path.ToArray()));
                }
                continue;
            }
            if (current.Path.Count >= minLength)
            {
                continue;
            }
            if (current.Point.X > 0)
            {
                var next = (current.Point.X - 1, current.Point.Y);
                if (next != forbidden && !current.Visited.Contains(next))
                {
                    queue.Enqueue((next, [..current.Path, '<'], [..current.Visited, next]));
                }
            }
            if (current.Point.X < 2)
            {
                var next = (current.Point.X + 1, current.Point.Y);
                if (next != forbidden && !current.Visited.Contains(next))
                {
                    queue.Enqueue((next, [..current.Path, '>'], [..current.Visited, next]));
                }
            }
            if (current.Point.Y > 0)
            {
                var next = (current.Point.X, current.Point.Y - 1);
                if (next != forbidden && !current.Visited.Contains(next))
                {
                    queue.Enqueue((next, [..current.Path, '^'], [..current.Visited, next]));
                }
            }
            if (current.Point.Y < 3)
            {
                var next = (current.Point.X, current.Point.Y + 1);
                if (next != forbidden && !current.Visited.Contains(next))
                {
                    queue.Enqueue((next, [..current.Path, 'v'], [..current.Visited, next]));
                }
            }
        }
        return [..paths];
    }

    public static string ParseLine(ReadOnlySpan<char> line)
    {
        return new string(line);
    }

    public static Solution FromParsed(IReadOnlyList<string> entries)
    {
        return new Solution(entries);
    }
}