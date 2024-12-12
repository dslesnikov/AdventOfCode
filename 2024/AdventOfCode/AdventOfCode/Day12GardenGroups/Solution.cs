namespace AdventOfCode.Day12GardenGroups;

public class Solution : IFromLines<Solution, IReadOnlyList<char>>
{
    public static int Day => 12;

    private readonly IReadOnlyList<IReadOnlyList<char>> _map;

    private Solution(IReadOnlyList<IReadOnlyList<char>> map)
    {
        _map = map;
    }

    public string SolvePartOne()
    {
        var result = CalculateFencePrice(GetSimplePerimeter);
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var result = CalculateFencePrice(GetComplexPerimeter);
        return result.ToString();
    }

    private static int GetSimplePerimeter(HashSet<(int Row, int Col)> tiles)
    {
        var result = 0;
        foreach (var tile in tiles)
        {
            var contribution = 4;
            if (tiles.Contains((tile.Row - 1, tile.Col)))
            {
                contribution--;
            }
            if (tiles.Contains((tile.Row + 1, tile.Col)))
            {
                contribution--;
            }
            if (tiles.Contains((tile.Row, tile.Col - 1)))
            {
                contribution--;
            }
            if (tiles.Contains((tile.Row, tile.Col + 1)))
            {
                contribution--;
            }
            result += contribution;
        }
        return result;
    }

    private static int GetComplexPerimeter(HashSet<(int Row, int Col)> tiles)
    {
        var rawPerimeterTiles = new HashSet<(int Row, int Col, int Side)>();
        foreach (var tile in tiles)
        {
            if (!tiles.Contains((tile.Row - 1, tile.Col)))
            {
                rawPerimeterTiles.Add((tile.Row - 1, tile.Col, 0));
            }
            if (!tiles.Contains((tile.Row + 1, tile.Col)))
            {
                rawPerimeterTiles.Add((tile.Row + 1, tile.Col, 2));
            }
            if (!tiles.Contains((tile.Row, tile.Col - 1)))
            {
                rawPerimeterTiles.Add((tile.Row, tile.Col - 1, 1));
            }
            if (!tiles.Contains((tile.Row, tile.Col + 1)))
            {
                rawPerimeterTiles.Add((tile.Row, tile.Col + 1, 3));
            }
        }

        var result = 0;
        while (rawPerimeterTiles.Count > 0)
        {
            var initialTile = rawPerimeterTiles.First();
            if (initialTile.Side % 2 != 0)
            {
                var tile = initialTile;
                while (rawPerimeterTiles.Remove((--tile.Row, tile.Col, tile.Side)))
                {
                }
                tile = initialTile;
                while (rawPerimeterTiles.Remove((++tile.Row, tile.Col, tile.Side)))
                {
                }
            }
            else
            {
                var tile = initialTile;
                while (rawPerimeterTiles.Remove((tile.Row, --tile.Col, tile.Side)))
                {
                }
                tile = initialTile;
                while (rawPerimeterTiles.Remove((tile.Row, ++tile.Col, tile.Side)))
                {
                }
            }
            rawPerimeterTiles.Remove(initialTile);
            result++;
        }
        return result;
    }

    private int CalculateFencePrice(Func<HashSet<(int, int)>, int> calculatePerimeter)
    {
        var result = 0;
        var mapped = new HashSet<(int, int)>();
        for (var row = 0; row < _map.Count; row++)
        {
            for (var col = 0; col < _map[row].Count; col++)
            {
                if (mapped.Contains((row, col)))
                {
                    continue;
                }
                var group = TraverseGroup(row, col);
                mapped.UnionWith(group);
                checked
                {
                    result += calculatePerimeter(group) * group.Count;
                }
            }
        }
        return result;
    }

    private HashSet<(int Row, int Col)> TraverseGroup(int row, int col)
    {
        var value = _map[row][col];
        var visited = new HashSet<(int, int)> {(row, col)};
        var queue = new Queue<(int Row, int Col)>();
        queue.Enqueue((row, col));
        Span<(int Row, int Col)> neighbors = stackalloc (int, int)[4];
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var length = GetNeighbors(current.Row, current.Col, value, neighbors);
            for (var i = 0; i < length; i++)
            {
                var neighbor = neighbors[i];
                if (visited.Add(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
        return visited;
    }

    private int GetNeighbors(int row, int col, char value, Span<(int, int)> neighbors)
    {
        var length = 0;
        if (row > 0 && _map[row - 1][col] == value)
        {
            neighbors[length++] = (row - 1, col);
        }
        if (row < _map.Count - 1 && _map[row + 1][col] == value)
        {
            neighbors[length++] = (row + 1, col);
        }
        if (col > 0 && _map[row][col - 1] == value)
        {
            neighbors[length++] = (row, col - 1);
        }
        if (col < _map[row].Count - 1 && _map[row][col + 1] == value)
        {
            neighbors[length++] = (row, col + 1);
        }
        return length;
    }

    public static IReadOnlyList<char> ParseLine(ReadOnlySpan<char> line)
    {
        return line.ToArray();
    }

    public static Solution FromParsed(IReadOnlyList<IReadOnlyList<char>> entries)
    {
        return new Solution(entries);
    }
}