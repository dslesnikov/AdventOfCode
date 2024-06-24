namespace AdventOfCode._16_TheFloorWillBeLava;

public class Solution(IReadOnlyList<IReadOnlyList<char>> field) : ISolution<Solution>
{
    public static int Day => 16;

    public static Solution Parse(string s)
    {
        var field = s.Split('\n')
            .Select(c => c.ToCharArray())
            .ToArray();
        return new Solution(field);
    }

    public string Part1()
    {
        var result = CountEnergizedTiles(0, 0, Direction.Right);
        return result.ToString();
    }

    public string Part2()
    {
        var leftRightStarts = Enumerable.Range(0, field.Count)
            .SelectMany(row => new[]
            {
                (Row: row, Col: 0, Direction: Direction.Right),
                (Row: row, Col: field[0].Count - 1, Direction: Direction.Left)
            });
        var topBottomStarts = Enumerable.Range(0, field[0].Count)
            .SelectMany(col => new[]
            {
                (Row: 0, Col: col, Direction: Direction.Down),
                (Row: field.Count - 1, Col: col, Direction: Direction.Up)
            });
        var starts = leftRightStarts.Concat(topBottomStarts);
        var result = starts.Max(start => CountEnergizedTiles(start.Row, start.Col, start.Direction));
        return result.ToString();
    }

    private int CountEnergizedTiles(int startRow, int startCol, Direction direction)
    {
        var energized = new bool[field.Count][];
        for (var i = 0; i < energized.Length; i++)
        {
            energized[i] = new bool[field[0].Count];
        }
        energized[startRow][startCol] = true;
        var visited = new Direction[field.Count][];
        for (var i = 0; i < field.Count; i++)
        {
            visited[i] = new Direction[field[0].Count];
        }
        var queue = new Queue<(int Row, int Col, Direction Direction)>();
        visited[startRow][startCol] |= direction;
        queue.Enqueue((startRow, startCol, direction));
        while (queue.TryDequeue(out var move))
        {
            var row = move.Row;
            var col = move.Col;
            switch (move.Direction)
            {
                case Direction.Up:
                    MoveUp(row, col, energized, visited, queue);
                    break;
                case Direction.Down:
                    MoveDown(row, col, energized, visited, queue);
                    break;
                case Direction.Left:
                    MoveLeft(row, col, energized, visited, queue);
                    break;
                case Direction.Right:
                    MoveRight(row, col, energized, visited, queue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return energized.Sum(row => row.Count(x => x));
    }

    private void MoveUp(int row, int col,
        bool[][] energized,
        Direction[][] visited,
        Queue<(int Row, int Col, Direction Direction)> queue)
    {
        while (row >= 0)
        {
            energized[row][col] = true;
            visited[row][col] |= Direction.Up;
            var stop = false;
            if (field[row][col] is '-' or '\\')
            {
                if (col > 0 && !visited[row][col - 1].HasFlag(Direction.Left))
                {
                    queue.Enqueue((row, col - 1, Direction.Left));
                    visited[row][col - 1] |= Direction.Left;
                }
                stop = true;
            }
            if (field[row][col] is '-' or '/')
            {
                if (col < field[row].Count - 1 && !visited[row][col + 1].HasFlag(Direction.Right))
                {
                    queue.Enqueue((row, col + 1, Direction.Right));
                    visited[row][col + 1] |= Direction.Right;
                }
                stop = true;
            }
            if (stop)
            {
                break;
            }
            row--;
        }
    }

    private void MoveDown(int row, int col,
        bool[][] energized,
        Direction[][] visited,
        Queue<(int Row, int Col, Direction Direction)> queue)
    {
        while (row < field.Count)
        {
            energized[row][col] = true;
            visited[row][col] |= Direction.Down;
            var stop = false;
            if (field[row][col] is '-' or '\\')
            {
                if (col < field[row].Count - 1 && !visited[row][col + 1].HasFlag(Direction.Right))
                {
                    queue.Enqueue((row, col + 1, Direction.Right));
                    visited[row][col + 1] |= Direction.Right;
                }
                stop = true;
            }
            if (field[row][col] is '-' or '/')
            {
                if (col > 0 && !visited[row][col - 1].HasFlag(Direction.Left))
                {
                    queue.Enqueue((row, col - 1, Direction.Left));
                    visited[row][col - 1] |= Direction.Left;
                }
                stop = true;
            }
            if (stop)
            {
                break;
            }
            row++;
        }
    }

    private void MoveLeft(int row, int col,
        bool[][] energized,
        Direction[][] visited,
        Queue<(int Row, int Col, Direction Direction)> queue)
    {
        while (col >= 0)
        {
            energized[row][col] = true;
            visited[row][col] |= Direction.Left;
            var stop = false;
            if (field[row][col] is '|' or '\\')
            {
                if (row > 0 && !visited[row - 1][col].HasFlag(Direction.Up))
                {
                    queue.Enqueue((row - 1, col, Direction.Up));
                    visited[row - 1][col] |= Direction.Up;
                }
                stop = true;
            }
            if (field[row][col] is '|' or '/')
            {
                if (row < field.Count - 1 && !visited[row + 1][col].HasFlag(Direction.Down))
                {
                    queue.Enqueue((row + 1, col, Direction.Down));
                    visited[row + 1][col] |= Direction.Down;
                }
                stop = true;
            }
            if (stop)
            {
                break;
            }
            col--;
        }
    }

    private void MoveRight(int row, int col,
        bool[][] energized,
        Direction[][] visited,
        Queue<(int Row, int Col, Direction Direction)> queue)
    {
        while (col < field[row].Count)
        {
            energized[row][col] = true;
            visited[row][col] |= Direction.Right;
            var stop = false;
            if (field[row][col] is '|' or '\\')
            {
                if (row < field.Count - 1 && !visited[row + 1][col].HasFlag(Direction.Down))
                {
                    queue.Enqueue((row + 1, col, Direction.Down));
                    visited[row + 1][col] |= Direction.Down;
                }
                stop = true;
            }
            if (field[row][col] is '|' or '/')
            {
                if (row > 0 && !visited[row - 1][col].HasFlag(Direction.Up))
                {
                    queue.Enqueue((row - 1, col, Direction.Up));
                    visited[row - 1][col] |= Direction.Up;
                }
                stop = true;
            }
            if (stop)
            {
                break;
            }
            col++;
        }
    }
}