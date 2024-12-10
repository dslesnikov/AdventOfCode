namespace AdventOfCode.Day10HoofIt;

public class Solution : IFromLines<Solution, IReadOnlyList<int>>
{
    public static int Day => 10;

    private readonly IReadOnlyList<IReadOnlyList<int>> _map;
    private readonly IReadOnlyList<(int Row, int Col)> _trailHeads;

    private Solution(IReadOnlyList<IReadOnlyList<int>> map)
    {
        _map = map;
        var trailHeads = new List<(int, int)>();
        for (var row = 0; row < map.Count; row++)
        {
            for (var col = 0; col < map[row].Count; col++)
            {
                if (map[row][col] == 0)
                {
                    trailHeads.Add((row, col));
                }
            }
        }
        _trailHeads = trailHeads;
    }

    public string SolvePartOne()
    {
        Span<(int Row, int Col)> neighbors = stackalloc (int, int)[4];
        var result = 0;
        var ends = new HashSet<(int, int)>();
        var queue = new Queue<(int Row, int Col, int Value)>();
        foreach (var start in _trailHeads)
        {
            ends.Clear();
            queue.Clear();
            queue.Enqueue((start.Row, start.Col, 0));
            while (queue.Count > 0)
            {
                var (row, col, value) = queue.Dequeue();
                if (value == 9)
                {
                    ends.Add((row, col));
                    continue;
                }
                var count = GetNeighbors(row, col, neighbors);
                for (var i = 0; i < count; i++)
                {
                    var neighbor = neighbors[i];
                    if (_map[neighbor.Row][neighbor.Col] == value + 1)
                    {
                        queue.Enqueue((neighbor.Row, neighbor.Col, value + 1));
                    }
                }
            }
            result += ends.Count;
        }
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        Span<(int Row, int Col)> neighbors = stackalloc (int, int)[4];
        var result = 0;
        var queue = new Queue<(int Row, int Col, int Value)>();
        foreach (var start in _trailHeads)
        {
            queue.Clear();
            queue.Enqueue((start.Row, start.Col, 0));
            var numberOfPaths = 0;
            while (queue.Count > 0)
            {
                var (row, col, value) = queue.Dequeue();
                if (value == 9)
                {
                    numberOfPaths++;
                    continue;
                }
                var count = GetNeighbors(row, col, neighbors);
                for (var i = 0; i < count; i++)
                {
                    var neighbor = neighbors[i];
                    if (_map[neighbor.Row][neighbor.Col] == value + 1)
                    {
                        queue.Enqueue((neighbor.Row, neighbor.Col, value + 1));
                    }
                }
            }
            result += numberOfPaths;
        }
        return result.ToString();
    }

    private int GetNeighbors(int row, int col, Span<(int Row, int Col)> neighbors)
    {
        var length = 0;
        if (row > 0)
        {
            neighbors[length++] = (row - 1, col);
        }
        if (row < _map.Count - 1)
        {
            neighbors[length++] = (row + 1, col);
        }
        if (col > 0)
        {
            neighbors[length++] = (row, col - 1);
        }
        if (col < _map[row].Count - 1)
        {
            neighbors[length++] = (row, col + 1);
        }
        return length;
    }

    public static IReadOnlyList<int> ParseLine(ReadOnlySpan<char> line)
    {
        var result = new int[line.Length];
        for (var i = 0; i < line.Length; i++)
        {
            result[i] = int.Parse([line[i]]);
        }
        return result;
    }

    public static Solution FromParsed(IReadOnlyList<IReadOnlyList<int>> entries)
    {
        return new Solution(entries);
    }
}