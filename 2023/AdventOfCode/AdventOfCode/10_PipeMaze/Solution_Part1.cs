namespace AdventOfCode._10_PipeMaze;

public partial class Solution
{
    public string Part1()
    {
        var loop = FindLoop();
        return (loop.Count / 2).ToString();
    }

    private IReadOnlySet<Cell> FindLoop()
    {
        var start = FindStart();
        var currentPath = new HashSet<Cell>();
        var stack = new Stack<Cell>();
        stack.Push(start);
        Span<Cell> neighbors = stackalloc Cell[4];
        var parents = new Dictionary<Cell, Cell>
        {
            [start] = start
        };
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            currentPath.Add(current);
            var neighborsLength = GetPipeNeighbors(current, neighbors);
            var isLast = true;
            for (var i = 0; i < neighborsLength; i++)
            {
                var neighbor = neighbors[i];
                if (neighbor.Equals(parents[current]))
                {
                    continue;
                }
                if (currentPath.Contains(neighbor))
                {
                    return currentPath;
                }
                parents[neighbor] = current;
                stack.Push(neighbor);
                isLast = false;
            }
            if (isLast)
            {
                currentPath.Remove(current);
            }
        }
        return new HashSet<Cell>();
    }

    private Cell FindStart()
    {
        var startRow = -1;
        var startCol = -1;
        for (var row = 0; row < field.Count; row++)
        {
            for (var col = 0; col < field[row].Count; col++)
            {
                if (field[row][col] != 'S')
                {
                    continue;
                }
                startRow = row;
                startCol = col;
                break;
            }
        }
        return new Cell(startRow, startCol);
    }

    private int GetPipeNeighbors(Cell cell, Span<Cell> neighbors)
    {
        var length = 0;
        if (cell.Row > 0 &&
            field[cell.Row][cell.Col] is 'S' or 'L' or 'J' or '|' &&
            field[cell.Row - 1][cell.Col] is 'S' or 'F' or '7' or '|')
        {
            neighbors[length++] = cell with { Row = cell.Row - 1 };
        }
        if (cell.Row < field.Count - 1 &&
            field[cell.Row][cell.Col] is 'S' or 'F' or '7' or '|' &&
            field[cell.Row + 1][cell.Col] is 'S' or 'L' or 'J' or '|')
        {
            neighbors[length++] = cell with { Row = cell.Row + 1 };
        }
        if (cell.Col > 0 &&
            field[cell.Row][cell.Col] is 'S' or '7' or 'J' or '-' &&
            field[cell.Row][cell.Col - 1] is 'S' or 'L' or 'F' or '-')
        {
            neighbors[length++] = cell with { Col = cell.Col - 1 };
        }
        if (cell.Col < field[cell.Row].Count - 1 &&
            field[cell.Row][cell.Col] is 'S' or 'L' or 'F' or '-' &&
            field[cell.Row][cell.Col + 1] is 'S' or '7' or 'J' or '-')
        {
            neighbors[length++] = cell with { Col = cell.Col + 1 };
        }
        return length;
    }
}