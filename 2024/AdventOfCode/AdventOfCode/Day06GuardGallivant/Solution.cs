using System.Diagnostics;

namespace AdventOfCode.Day06GuardGallivant;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public class Solution : IFromLines<Solution, string>
{
    public static int Day => 6;
    private readonly IReadOnlyList<string> _map;
    private readonly (int Row, int Col) _start;

    private Solution(IReadOnlyList<string> map)
    {
        _map = map;
        for (var row = 0; row < _map.Count; row++)
        {
            for (var col = 0; col < _map[row].Length; col++)
            {
                if (_map[row][col] == '^')
                {
                    _start = (row, col);
                    break;
                }
            }
        }
    }

    public string SolvePartOne()
    {
        var result = TraverseOnce(_start, Direction.Up);
        return result.Visited.DistinctBy(x => x.Item1).Count().ToString();
    }

    public string SolvePartTwo()
    {
        var (_, path) = TraverseOnce(_start, Direction.Up);
        var obstacles = new HashSet<(int, int)>();
        foreach (var (position, _) in path)
        {
            if (position == _start)
            {
                continue;
            }
            var isLoop = TraverseOnce(_start, Direction.Up, position).IsLoop;
            if (isLoop)
            {
                obstacles.Add(position);
            }
        }
        return obstacles.Count.ToString();
    }

    private (bool IsLoop, HashSet<((int, int), Direction)> Visited) TraverseOnce(
        (int Row, int Col) start,
        Direction direction,
        (int Row, int Col)? obstacle = null)
    {
        var current = start;
        var next = Next(current, direction);
        var visited = new HashSet<((int, int), Direction)>
        {
            (current, direction)
        };
        while (next.Row >= 0 &&
               next.Row <= _map.Count - 1 &&
               next.Col >= 0 &&
               next.Col <= _map[next.Row].Length - 1)
        {
            if (_map[next.Row][next.Col] != '#' &&
                next != obstacle)
            {
                current = next;
                if (!visited.Add((current, direction)))
                {
                    return (IsLoop: true, Visited: visited);
                }
            }
            else
            {
                direction = (Direction)(((int)direction + 1) % 4);
            }
            next = Next(current, direction);
        }
        return (IsLoop: false, Visited: visited);
    }

    public static string ParseLine(ReadOnlySpan<char> line)
    {
        return line.ToString();
    }

    public static Solution FromParsed(IReadOnlyList<string> entries)
    {
        return new Solution(entries);
    }

    private static (int Row, int Col) Next((int Row, int Col) position, Direction direction)
    {
        return direction switch
        {
            Direction.Up => (Row: position.Row - 1, position.Col),
            Direction.Right => (Row: position.Row, position.Col + 1),
            Direction.Down => (Row: position.Row + 1, position.Col),
            Direction.Left => (Row: position.Row, position.Col - 1),
            _ => throw new UnreachableException()
        };
    }
}