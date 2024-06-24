namespace AdventOfCode._17_ClumsyCrucible;

public class Solution(IReadOnlyList<IReadOnlyList<byte>> field) : ISolution<Solution>
{
    private const int UltraCrucibleInertia = 4;

    public static int Day => 17;

    public static Solution Parse(string s)
    {
        var field = s.Split('\n')
            .Select(line => line.Select(c => (byte)(c - '0')).ToArray())
            .ToArray();
        return new Solution(field);
    }

    public string Part1()
    {
        var heatLoss = GetHeatLoss(ValidateCrucibleMove, 0);
        return heatLoss.ToString();
    }

    public string Part2()
    {
        var heatLoss = GetHeatLoss(ValidateUltraCrucibleMove, UltraCrucibleInertia);
        return heatLoss.ToString();
    }

    private int GetHeatLoss(Func<TraversingState, Direction, bool> validateMove, int minSteps)
    {
        var visited = new Dictionary<(Direction, int), int>[field.Count][];
        for (var i = 0; i < visited.Length; i++)
        {
            visited[i] = new Dictionary<(Direction, int), int>[field[i].Count];
            for (var j = 0; j < visited[i].Length; j++)
            {
                visited[i][j] = new Dictionary<(Direction, int), int>();
            }
        }
        var queue = new PriorityQueue<TraversingState, int>();
        visited[0][0] = new Dictionary<(Direction, int), int>();
        queue.Enqueue(new TraversingState(0, 0, 0, Direction.Right, 0), 0);
        while (queue.TryDequeue(out var current, out _))
        {
            if (current.Row == field.Count - 1 &&
                current.Col == field[current.Row].Count - 1 &&
                current.Steps >= minSteps)
            {
                return current.HeatLoss;
            }
            if (visited[current.Row][current.Col].TryGetValue((current.Direction, current.Steps), out var distance) &&
                distance <= current.HeatLoss)
            {
                continue;
            }
            visited[current.Row][current.Col][(current.Direction, current.Steps)] = current.HeatLoss;
            foreach (var neighbor in GetNeighbors(current, validateMove))
            {
                if (visited[neighbor.Row][neighbor.Col].TryGetValue((neighbor.Direction, neighbor.Steps), out distance) &&
                    distance <= neighbor.HeatLoss)
                {
                    continue;
                }
                queue.Enqueue(neighbor, neighbor.HeatLoss);
            }
        }
        return -1;
    }

    private IEnumerable<TraversingState> GetNeighbors(TraversingState state, Func<TraversingState, Direction, bool> validateMove)
    {
        if (state.Row > 0 &&
            validateMove(state, Direction.Up))
        {
            var newRow = state.Row - 1;
            var newCol = state.Col;
            var newHeatLoss = state.HeatLoss + field[newRow][newCol];
            yield return new TraversingState(
                newRow,
                newCol,
                newHeatLoss,
                Direction.Up,
                state.Direction == Direction.Up ? state.Steps + 1 : 1);
        }
        if (state.Row < field.Count - 1 &&
            validateMove(state, Direction.Down))
        {
            var newRow = state.Row + 1;
            var newCol = state.Col;
            var newHeatLoss = state.HeatLoss + field[newRow][newCol];
            yield return new TraversingState(
                newRow,
                newCol,
                newHeatLoss,
                Direction.Down,
                state.Direction == Direction.Down ? state.Steps + 1 : 1);
        }
        if (state.Col > 0 &&
            validateMove(state, Direction.Left))
        {
            var newRow = state.Row;
            var newCol = state.Col - 1;
            var newHeatLoss = state.HeatLoss + field[newRow][newCol];
            yield return new TraversingState(
                newRow,
                newCol,
                newHeatLoss,
                Direction.Left,
                state.Direction == Direction.Left ? state.Steps + 1 : 1);
        }
        if (state.Col < field[0].Count - 1 &&
            validateMove(state, Direction.Right))
        {
            var newRow = state.Row;
            var newCol = state.Col + 1;
            var newHeatLoss = state.HeatLoss + field[newRow][newCol];
            yield return new TraversingState(
                newRow,
                newCol,
                newHeatLoss,
                Direction.Right,
                state.Direction == Direction.Right ? state.Steps + 1 : 1);
        }
    }

    private static bool ValidateCrucibleMove(TraversingState state, Direction direction)
    {
        const int crucibleInertia = 3;
        return direction switch
        {
            Direction.Up => state.Direction != Direction.Down &&
                            (state.Direction != Direction.Up || state.Steps < crucibleInertia),
            Direction.Down => state.Direction != Direction.Up &&
                              (state.Direction != Direction.Down || state.Steps < crucibleInertia),
            Direction.Left => state.Direction != Direction.Right &&
                              (state.Direction != Direction.Left || state.Steps < crucibleInertia),
            Direction.Right => state.Direction != Direction.Left &&
                               (state.Direction != Direction.Right || state.Steps < crucibleInertia),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static bool ValidateUltraCrucibleMove(TraversingState state, Direction direction)
    {
        if (state is { Row: 0, Col: 0 })
        {
            return true;
        }
        const int ultraCrucibleMaxInertia = 10;
        return direction switch
        {
            Direction.Up when state.Direction == Direction.Up => state.Steps < ultraCrucibleMaxInertia,
            Direction.Up => state.Direction != Direction.Down && state.Steps >= UltraCrucibleInertia,
            Direction.Down when state.Direction == Direction.Down => state.Steps < ultraCrucibleMaxInertia,
            Direction.Down => state.Direction != Direction.Up && state.Steps >= UltraCrucibleInertia,
            Direction.Left when state.Direction == Direction.Left => state.Steps < ultraCrucibleMaxInertia,
            Direction.Left => state.Direction != Direction.Right && state.Steps >= UltraCrucibleInertia,
            Direction.Right when state.Direction == Direction.Right => state.Steps < ultraCrucibleMaxInertia,
            Direction.Right => state.Direction != Direction.Left && state.Steps >= UltraCrucibleInertia,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}