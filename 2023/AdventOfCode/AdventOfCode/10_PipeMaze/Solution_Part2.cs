namespace AdventOfCode._10_PipeMaze;

public partial class Solution
{
    public string Part2()
    {
        var loop = FindLoop();
        var cleanedField = field
            .Select((rowTiles, row) => rowTiles
                .Select((tile, col) => loop.Contains(new Cell(row, col)) ? tile : '.')
                .ToArray())
            .ToArray();
        var potentiallyEnclosed = FindEnclosedTiles(loop, cleanedField);
        var expandedField = ExpandField(loop);
        var freeCells = FindFreeCells(expandedField);
        var enclosed = new HashSet<Cell>();
        var count = potentiallyEnclosed
            .Select(cell => new Cell(cell.Row * 2, cell.Col * 2))
            .Count(expanded => IsEnclosed(expanded, expandedField, enclosed, freeCells));
        return count.ToString();
    }

    private static IReadOnlySet<Cell> FindEnclosedTiles(
        IReadOnlySet<Cell> loop,
        IReadOnlyList<IReadOnlyList<char>> field)
    {
        var potentiallyEnclosed = new HashSet<Cell>();
        var freeCells = FindFreeCells(field);
        for (var row = 0; row < field.Count; row++)
        {
            for (var col = 0; col < field[row].Count; col++)
            {
                var cell = new Cell(row, col);
                if (loop.Contains(cell))
                {
                    continue;
                }
                var isEnclosed = IsEnclosed(cell, field, potentiallyEnclosed, freeCells);
                if (!isEnclosed)
                {
                    continue;
                }
                potentiallyEnclosed.Add(cell);
            }
        }
        return potentiallyEnclosed;
    }

    private static HashSet<Cell> FindFreeCells(IReadOnlyList<IReadOnlyList<char>> field)
    {
        var starts = new[] { new Cell(0, 0), new Cell(0, field[0].Count - 1) };
        var visited = new HashSet<Cell>();
        Span<Cell> neighbors = stackalloc Cell[4];
        foreach (var start in starts)
        {
            var queue = new Queue<Cell>();
            queue.Enqueue(start);
            visited.Add(start);
            while (queue.TryDequeue(out var current))
            {
                var neighborsCount = GetNeighbors(current, neighbors, field);
                for (var i = 0; i < neighborsCount; i++)
                {
                    var neighbor = neighbors[i];
                    if (field[neighbor.Row][neighbor.Col] != '.' ||
                        !visited.Add(neighbor))
                    {
                        continue;
                    }
                    queue.Enqueue(neighbor);
                }
            }
        }
        return visited;
    }

    private char[][] ExpandField(IReadOnlySet<Cell> loop)
    {
        var expandedField = new char[field.Count * 2 - 1][];
        for (var row = 0; row < expandedField.Length; row++)
        {
            expandedField[row] = new char[field[0].Count * 2 - 1];
            Array.Fill(expandedField[row], '#');
        }
        for (var row = 0; row < field.Count; row++)
        {
            for (var col = 0; col < field[row].Count; col++)
            {
                expandedField[row * 2][col * 2] = loop.Contains(new Cell(row, col))
                    ? field[row][col]
                    : '.';
            }
        }
        for (var row = 0; row < expandedField.Length; row++)
        {
            for (var col = 0; col < expandedField[row].Length; col++)
            {
                if (expandedField[row][col] == '#')
                {
                    expandedField[row][col] = TransformExpandedTile(new Cell(row, col), expandedField);
                }
            }
        }
        return expandedField;
    }

    private static bool IsEnclosed(
        Cell cell,
        IReadOnlyList<IReadOnlyList<char>> field,
        HashSet<Cell> enclosed,
        HashSet<Cell> freeCells)
    {
        if (cell.Row == 0 || cell.Row == field.Count - 1 ||
            cell.Col == 0 || cell.Col == field[cell.Row].Count - 1 ||
            freeCells.Contains(cell))
        {
            return false;
        }
        if (enclosed.Contains(cell))
        {
            return true;
        }
        var visited = new HashSet<Cell>();
        var queue = new Queue<Cell>();
        queue.Enqueue(cell);
        visited.Add(cell);
        Span<Cell> neighbors = stackalloc Cell[4];
        while (queue.TryDequeue(out var current))
        {
            var neighborsCount = GetNeighbors(current, neighbors, field);
            for (var i = 0; i < neighborsCount; i++)
            {
                var neighbor = neighbors[i];
                if (freeCells.Contains(neighbor))
                {
                    freeCells.UnionWith(visited);
                    return false;
                }
                if (enclosed.Contains(neighbor))
                {
                    enclosed.UnionWith(visited);
                    return true;
                }
                if (field[neighbor.Row][neighbor.Col] != '.' ||
                    !visited.Add(neighbor))
                {
                    continue;
                }
                queue.Enqueue(neighbor);
            }
        }
        enclosed.UnionWith(visited);
        return true;
    }

    private static char TransformExpandedTile(Cell cell, char[][] expandedField)
    {
        if (IsVerticallyConnecting(cell, expandedField))
        {
            return '|';
        }
        if (IsHorizontallyConnecting(cell, expandedField))
        {
            return '-';
        }
        return '.';
    }

    private static bool IsVerticallyConnecting(Cell cell, char[][] expandedField)
    {
        if (cell.Row == 0 || cell.Row == expandedField.Length - 1)
        {
            return false;
        }
        var above = expandedField[cell.Row - 1][cell.Col];
        var below = expandedField[cell.Row + 1][cell.Col];
        return above is '|' or '7' or 'F' or 'S' && below is '|' or 'J' or 'L' or 'S';
    }

    private static bool IsHorizontallyConnecting(Cell cell, char[][] expandedField)
    {
        if (cell.Col == 0 || cell.Col == expandedField[cell.Row].Length - 1)
        {
            return false;
        }
        var left = expandedField[cell.Row][cell.Col - 1];
        var right = expandedField[cell.Row][cell.Col + 1];
        return left is '-' or 'L' or 'F' or 'S' && right is '-' or 'J' or '7' or 'S';
    }

    private static int GetNeighbors(
        Cell cell,
        Span<Cell> neighbors,
        IReadOnlyList<IReadOnlyList<char>> field)
    {
        var length = 0;
        if (cell.Row > 0)
        {
            neighbors[length++] = cell with { Row = cell.Row - 1 };
        }
        if (cell.Row < field.Count - 1)
        {
            neighbors[length++] = cell with { Row = cell.Row + 1 };
        }
        if (cell.Col > 0)
        {
            neighbors[length++] = cell with { Col = cell.Col - 1 };
        }
        if (cell.Col < field[cell.Row].Count - 1)
        {
            neighbors[length++] = cell with { Col = cell.Col + 1 };
        }
        return length;
    }
}