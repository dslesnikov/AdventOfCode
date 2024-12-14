using System.Text.RegularExpressions;

namespace AdventOfCode.Day14RestroomRedoubt;

public static partial class Regexes
{
    [GeneratedRegex("-?\\d+")]
    public static partial Regex Number { get; }
}

public readonly record struct Point(int X, int Y)
{
    public Point Move(Vector velocity, int time, int height, int width)
    {
        var targetX = (X + velocity.X * time) % width;
        targetX = targetX < 0 ? targetX + width : targetX;
        var targetY = (Y + velocity.Y * time) % height;
        targetY = targetY < 0 ? targetY + height : targetY;
        return new Point(targetX, targetY);
    }

    public static Point FromString(ReadOnlySpan<char> line)
    {
        var numberMatches = Regexes.Number.EnumerateMatches(line);
        numberMatches.MoveNext();
        var x = int.Parse(line.Slice(numberMatches.Current.Index, numberMatches.Current.Length));
        numberMatches.MoveNext();
        var y = int.Parse(line.Slice(numberMatches.Current.Index, numberMatches.Current.Length));
        return new Point(x, y);
    }
}

public record Vector(int X, int Y)
{
    public static Vector FromString(ReadOnlySpan<char> line)
    {
        var numberMatches = Regexes.Number.EnumerateMatches(line);
        numberMatches.MoveNext();
        var x = int.Parse(line.Slice(numberMatches.Current.Index, numberMatches.Current.Length));
        numberMatches.MoveNext();
        var y = int.Parse(line.Slice(numberMatches.Current.Index, numberMatches.Current.Length));
        return new Vector(x, y);
    }
}

public record Robot(Point Start, Vector Velocity)
{
    public static Robot FromString(ReadOnlySpan<char> line)
    {
        var split = line.Split(' ');
        split.MoveNext();
        var start = Point.FromString(line[split.Current.Start..split.Current.End]);
        split.MoveNext();
        var velocity = Vector.FromString(line[split.Current.Start..split.Current.End]);
        return new Robot(start, velocity);
    }
}

public class Solution : IFromLines<Solution, Robot>
{
    public static int Day => 14;

    private readonly IReadOnlyList<Robot> _robots;

    private Solution(IReadOnlyList<Robot> robots)
    {
        _robots = robots;
    }

    public string SolvePartOne()
    {
        var topLeft = 0;
        var topRight = 0;
        var bottomLeft = 0;
        var bottomRight = 0;
        const int height = 103;
        const int width = 101;
        const int middleY = 51;
        const int middleX = 50;
        foreach (var robot in _robots)
        {
            var result = robot.Start.Move(robot.Velocity, 100, height, width);
            switch (result)
            {
                case { X: < middleX, Y: < middleY }:
                    topLeft++;
                    break;
                case { X: > middleX, Y: < middleY }:
                    topRight++;
                    break;
                case { X: < middleX, Y: > middleY }:
                    bottomLeft++;
                    break;
                case { X: > middleX, Y: > middleY }:
                    bottomRight++;
                    break;
            }
        }
        return (topLeft * topRight * bottomLeft * bottomRight).ToString();
    }

    public string SolvePartTwo()
    {
        const int height = 103;
        const int width = 101;
        var time = 1;
        while (time < 1_000_000)
        {
            var robotPositions = _robots
                .Select(x => x.Start.Move(x.Velocity, time, height, width))
                .ToHashSet();
            var goodColumns = 0;
            for (var col = 0; col < width; col++)
            {
                for (var row = 0; row < height - 8; row += 8)
                {
                    if (robotPositions.Contains(new Point(col, row)) &&
                        robotPositions.Contains(new Point(col, row + 1)) &&
                        robotPositions.Contains(new Point(col, row + 2)) &&
                        robotPositions.Contains(new Point(col, row + 3)) &&
                        robotPositions.Contains(new Point(col, row + 4)) &&
                        robotPositions.Contains(new Point(col, row + 5)) &&
                        robotPositions.Contains(new Point(col, row + 6)) &&
                        robotPositions.Contains(new Point(col, row + 7)))
                    {
                        goodColumns++;
                    }
                }
            }

            if (goodColumns >= 3)
            {
                Console.Clear();
                Visualize(width, height, robotPositions);
                Console.WriteLine("Seems right?");
                var response = Console.ReadLine();
                if (response == "yes")
                {
                    return time.ToString();
                }
            }
            time++;
        }

        return "no";
    }

    private static void Visualize(int width, int height, HashSet<Point> robots)
    {
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                Console.Write(robots.Contains(new Point(j, i)) ? '#' : '.');
            }
            Console.WriteLine();
        }
    }

    public static Robot ParseLine(ReadOnlySpan<char> line)
    {
        return Robot.FromString(line);
    }

    public static Solution FromParsed(IReadOnlyList<Robot> entries)
    {
        return new Solution(entries);
    }
}