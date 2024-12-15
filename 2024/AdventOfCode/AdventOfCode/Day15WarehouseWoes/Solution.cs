namespace AdventOfCode.Day15WarehouseWoes;

public class Solution : IFromText<Solution>
{
    public static int Day => 15;

    private readonly IReadOnlyList<IReadOnlyList<Tile>> _map;
    private readonly IReadOnlyList<Direction> _commands;
    private readonly (int Row, int Col) _start;

    private Solution(IReadOnlyList<IReadOnlyList<Tile>> map, IReadOnlyList<Direction> commands)
    {
        _map = map;
        _commands = commands;
        for (var row = 0; row < map.Count; row++)
        {
            for (var col = 0; col < map[row].Count; col++)
            {
                if (map[row][col] == Tile.Robot)
                {
                    _start = (row, col);
                    return;
                }
            }
        }
    }

    public string SolvePartOne()
    {
        var map = _map.Select(x => x.ToArray()).ToArray();
        var robot = _start;
        foreach (var command in _commands)
        {
            var target = Move(robot, command);
            if (map[target.Row][target.Col] == Tile.Empty)
            {
                map[robot.Row][robot.Col] = Tile.Empty;
                map[target.Row][target.Col] = Tile.Robot;
                robot = target;
                continue;
            }
            var anyBoxes = false;
            while (map[target.Row][target.Col] == Tile.Box)
            {
                anyBoxes = true;
                target = Move(target, command);
            }
            if (map[target.Row][target.Col] == Tile.Wall)
            {
                continue;
            }
            if (anyBoxes)
            {
                map[target.Row][target.Col] = Tile.Box;
                map[robot.Row][robot.Col] = Tile.Empty;
                var neighbor = Move(robot, command);
                map[neighbor.Row][neighbor.Col] = Tile.Robot;
                robot = neighbor;
            }
            else
            {
                map[robot.Row][robot.Col] = Tile.Empty;
                map[target.Row][target.Col] = Tile.Robot;
                robot = target;
            }
        }

        var result = 0;
        for (var row = 0; row < map.Length; row++)
        {
            for (var col = 0; col < map[row].Length; col++)
            {
                var tile = map[row][col];
                if (tile != Tile.Box)
                {
                    continue;
                }
                checked
                {
                    result += row * 100 + col;
                }
            }
        }
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var map = _map
            .Select(x => x
                .SelectMany(tile => tile switch
                {
                    Tile.Empty => new[] { ExtendedTile.Empty, ExtendedTile.Empty },
                    Tile.Wall => new[] { ExtendedTile.Wall, ExtendedTile.Wall },
                    Tile.Box => new[] { ExtendedTile.BoxLeft, ExtendedTile.BoxRight },
                    Tile.Robot => new[] { ExtendedTile.Robot, ExtendedTile.Empty },
                    _ => throw new NotSupportedException()
                })
                .ToArray())
            .ToArray();
        var start = (Row: -1, Col: -1);
        for (var row = 0; row < map.Length; row++)
        {
            for (var col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] == ExtendedTile.Robot)
                {
                    start = (row, col);
                    break;
                }
            }
        }
        var robot = start;
        foreach (var command in _commands)
        {
            var target = Move(robot, command);
            if (map[target.Row][target.Col] == ExtendedTile.Wall)
            {
                continue;
            }
            if (map[target.Row][target.Col] == ExtendedTile.Empty)
            {
                map[robot.Row][robot.Col] = ExtendedTile.Empty;
                map[target.Row][target.Col] = ExtendedTile.Robot;
                robot = target;
                continue;
            }
            var (targetSurface, sourceSurface) = FindAffectedSurface(map, robot, command);
            if (targetSurface.Any(point => map[point.Row][point.Col] == ExtendedTile.Wall))
            {
                continue;
            }
            var reverse = Reverse(command);
            foreach (var point in targetSurface)
            {
                var tile = point;
                var before = Move(point, reverse);
                while (!sourceSurface.Contains(before))
                {
                    (map[tile.Row][tile.Col], map[before.Row][before.Col]) = (map[before.Row][before.Col], map[tile.Row][tile.Col]);
                    tile = before;
                    before = Move(tile, reverse);
                }
                (map[tile.Row][tile.Col], map[before.Row][before.Col]) = (map[before.Row][before.Col], map[tile.Row][tile.Col]);
            }
            map[robot.Row][robot.Col] = ExtendedTile.Empty;
            var neighbor = Move(robot, command);
            map[neighbor.Row][neighbor.Col] = ExtendedTile.Robot;
            robot = neighbor;
        }

        var result = 0;
        for (var row = 0; row < map.Length; row++)
        {
            for (var col = 0; col < map[row].Length; col++)
            {
                var tile = map[row][col];
                if (tile != ExtendedTile.BoxLeft)
                {
                    continue;
                }
                checked
                {
                    result += row * 100 + col;
                }
            }
        }
        return result.ToString();
    }

    private (IReadOnlyList<(int Row, int Col)> TargetSurface, IReadOnlySet<(int Row, int Col)> SourceSurface) FindAffectedSurface(
        ExtendedTile[][] map,
        (int Row, int Col) robot,
        Direction direction)
    {
        if (direction is Direction.Left or Direction.Right)
        {
            var targetTile = Move(robot, direction);
            while (map[targetTile.Row][targetTile.Col] is ExtendedTile.BoxLeft or ExtendedTile.BoxRight)
            {
                targetTile = Move(targetTile, direction);
            }
            return ([targetTile], new HashSet<(int Row, int Col)>{ Move(robot, direction) });
        }
        var queue = new Queue<(int Row, int Col)>();
        var newVacantTiles = new HashSet<(int Row, int Col)>();
        var target = Move(robot, direction);
        var visited = new HashSet<(int Row, int Col)> { target };
        var tile = map[target.Row][target.Col];
        newVacantTiles.Add(target);
        queue.Enqueue(target);
        if (tile is ExtendedTile.BoxLeft)
        {
            var move = Move(target, Direction.Right);
            queue.Enqueue(move);
            newVacantTiles.Add(move);
            visited.Add(move);
        }
        else if (tile is ExtendedTile.BoxRight)
        {
            var move = Move(target, Direction.Left);
            queue.Enqueue(move);
            newVacantTiles.Add(move);
            visited.Add(move);
        }
        var surface = new HashSet<(int Row, int Col)>();
        var reverse = Reverse(direction);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            target = Move(current, direction);
            tile = map[target.Row][target.Col];
            if (tile is ExtendedTile.BoxLeft)
            {
                if (visited.Add(target))
                {
                    queue.Enqueue(target);
                }
                var otherHalf = Move(target, Direction.Right);
                if (visited.Add(otherHalf))
                {
                    queue.Enqueue(otherHalf);
                }
                if (!visited.Contains(Move(otherHalf, reverse)))
                {
                    newVacantTiles.Add(otherHalf);
                }
            }
            else if (tile is ExtendedTile.BoxRight)
            {
                if (visited.Add(target))
                {
                    queue.Enqueue(target);
                }
                var otherHalf = Move(target, Direction.Left);
                if (visited.Add(otherHalf))
                {
                    queue.Enqueue(otherHalf);
                }
                if (!visited.Contains(Move(otherHalf, reverse)))
                {
                    newVacantTiles.Add(otherHalf);
                }
            }
            else
            {
                surface.Add(target);
            }
        }
        return (surface.ToArray(), newVacantTiles);
    }

    private static Direction Reverse(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new NotSupportedException()
        };
    }

    private static (int Row, int Col) Move((int Row, int Col) current, Direction direction)
    {
        return direction switch
        {
            Direction.Up => (current.Row - 1, current.Col),
            Direction.Down => (current.Row + 1, current.Col),
            Direction.Left => (current.Row, current.Col - 1),
            Direction.Right => (current.Row, current.Col + 1),
            _ => throw new NotSupportedException()
        };
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n");
        var map = split[0]
            .Split('\n')
            .Select(IReadOnlyList<Tile> (line) => line
                .Select(c => c switch
                {
                    '.' => Tile.Empty,
                    '#' => Tile.Wall,
                    'O' => Tile.Box,
                    '@' => Tile.Robot,
                    _ => throw new InvalidOperationException()
                })
                .ToArray())
            .ToArray();
        var commands = split[1]
            .Where(x => x != '\n')
            .Select(c => c switch
            {
                '<' => Direction.Left,
                '>' => Direction.Right,
                '^' => Direction.Up,
                'v' => Direction.Down,
                _ => throw new NotSupportedException()
            })
            .ToArray();
        return new Solution(map, commands);
    }
}