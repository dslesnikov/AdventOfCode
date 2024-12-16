namespace AdventOfCode.Day16ReindeerMaze;

public enum Tile
{
    Empty,
    Wall,
    Start,
    End
}

public enum Direction
{
    North,
    East,
    South,
    West
}

public class Solution : IFromLines<Solution, IReadOnlyList<Tile>>
{
    public static int Day => 16;

    private readonly IReadOnlyList<IReadOnlyList<Tile>> _map;
    private readonly (int Row, int Col) _start;
    private readonly (int Row, int Col) _end;

    private Solution(IReadOnlyList<IReadOnlyList<Tile>> map)
    {
        _map = map;
        for (var row = 0; row < map.Count; row++)
        {
            for (var col = 0; col < map[row].Count; col++)
            {
                switch (map[row][col])
                {
                    case Tile.Start:
                        _start = (row, col);
                        break;
                    case Tile.End:
                        _end = (row, col);
                        break;
                }
            }
        }
    }

    public string SolvePartOne()
    {
        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(new(_start.Row, _start.Col, Direction.East, 0), 0);
        var visited = new Dictionary<(int Row, int Col, Direction Direction), int>();
        Span<State> nextStates = stackalloc State[3];
        while (queue.TryDequeue(out var current, out var score))
        {
            if (current.Row == _end.Row && current.Col == _end.Col)
            {
                return score.ToString();
            }
            if (!visited.TryAdd((current.Row, current.Col, current.Direction), score))
            {
                continue;
            }
            var length = GetNextStates(current, nextStates);
            for (var i = 0; i < length; i++)
            {
                var nextState = nextStates[i];
                if (!visited.TryGetValue((nextState.Row, nextState.Col, nextState.Direction), out var nextScore) || nextState.Score < nextScore)
                {
                    queue.Enqueue(nextState, nextState.Score);
                }
            }
        }
        throw new InvalidOperationException();
    }

    public string SolvePartTwo()
    {
        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(new(_start.Row, _start.Col, Direction.East, 0), 0);
        var visited = new Dictionary<(int Row, int Col, Direction Direction), int>();
        Span<State> nextStates = stackalloc State[3];
        var final = (Row: -1, Col: -1, Direction: Direction.North);
        while (queue.TryDequeue(out var current, out var score))
        {
            if (current.Row == _end.Row && current.Col == _end.Col)
            {
                final = (current.Row, current.Col, current.Direction);
                visited.TryAdd((current.Row, current.Col, current.Direction), score);
                break;
            }
            if (!visited.TryAdd((current.Row, current.Col, current.Direction), score))
            {
                continue;
            }
            var length = GetNextStates(current, nextStates);
            for (var i = 0; i < length; i++)
            {
                var nextState = nextStates[i];
                if (!visited.TryGetValue((nextState.Row, nextState.Col, nextState.Direction), out var nextScore) || nextState.Score < nextScore)
                {
                    queue.Enqueue(nextState, nextState.Score);
                }
            }
        }
        var pathTiles = CountTilesInPaths(visited, final);
        return pathTiles.ToString();
    }

    private int CountTilesInPaths(
        Dictionary<(int Row, int Col, Direction Direction), int> visited,
        (int Row, int Col, Direction Direction) final)
    {
        var tiles = new HashSet<(int Row, int Col)>();
        var finalScore = visited[final];
        var queue = new Queue<(int Row, int Col, Direction Direction, int Score)>();
        queue.Enqueue((final.Row, final.Col, final.Direction, finalScore));
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            tiles.Add((current.Row, current.Col));
            var moveBack = Move(current.Row, current.Col, Reverse(current.Direction));
            if (visited.TryGetValue((moveBack.Row, moveBack.Col, current.Direction), out var score) && score == current.Score - 1)
            {
                queue.Enqueue((moveBack.Row, moveBack.Col, current.Direction, score));
            }
            var clockwise = RotateClockwise(current.Direction);
            if (visited.TryGetValue((current.Row, current.Col, clockwise), out score) && score == current.Score - 1000)
            {
                queue.Enqueue((current.Row, current.Col, clockwise, score));
            }
            var counterClockwise = RotateCounterClockwise(current.Direction);
            if (visited.TryGetValue((current.Row, current.Col, counterClockwise), out score) && score == current.Score - 1000)
            {
                queue.Enqueue((current.Row, current.Col, counterClockwise, score));
            }
        }
        return tiles.Count;
    }

    private int GetNextStates(State current, Span<State> nextStates)
    {
        var length = 0;
        var (row, col) = Move(current.Row, current.Col, current.Direction);
        if (row >= 0 &&
            row < _map.Count &&
            col >= 0 &&
            col < _map[row].Count &&
            _map[row][col] != Tile.Wall)
        {
            nextStates[length++] = new(row, col, current.Direction, current.Score + 1);
        }
        var clockwise = RotateClockwise(current.Direction);
        var clockwiseMove = Move(current.Row, current.Col, clockwise);
        if (clockwiseMove.Row >= 0 &&
            clockwiseMove.Row < _map.Count &&
            clockwiseMove.Col >= 0 &&
            clockwiseMove.Col < _map[clockwiseMove.Row].Count &&
            _map[clockwiseMove.Row][clockwiseMove.Col] != Tile.Wall)
        {
            nextStates[length++] = current with { Direction = clockwise, Score = current.Score + 1000 };
        }
        var counterClockwise = RotateCounterClockwise(current.Direction);
        var counterClockwiseMove = Move(current.Row, current.Col, counterClockwise);
        if (counterClockwiseMove.Row >= 0 &&
            counterClockwiseMove.Row < _map.Count &&
            counterClockwiseMove.Col >= 0 &&
            counterClockwiseMove.Col < _map[counterClockwiseMove.Row].Count &&
            _map[counterClockwiseMove.Row][counterClockwiseMove.Col] != Tile.Wall)
        {
            nextStates[length++] = current with { Direction = counterClockwise, Score = current.Score + 1000 };
        }
        return length;
    }

    private static (int Row, int Col) Move(int row, int col, Direction direction)
    {
        return direction switch
        {
            Direction.North => (row - 1, col),
            Direction.East => (row, col + 1),
            Direction.South => (row + 1, col),
            Direction.West => (row, col - 1),
            _ => throw new InvalidOperationException()
        };
    }

    private static Direction RotateClockwise(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new InvalidOperationException()
        };
    }

    private static Direction RotateCounterClockwise(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.West,
            Direction.West => Direction.South,
            Direction.South => Direction.East,
            Direction.East => Direction.North,
            _ => throw new InvalidOperationException()
        };
    }

    private static Direction Reverse(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.West => Direction.East,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            _ => throw new InvalidOperationException()
        };
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

    private readonly record struct State(int Row, int Col, Direction Direction, int Score);
}