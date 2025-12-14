namespace AdventOfCode.Day12ChristmasTreeFarm;

public record Region(int Width, int Length, IReadOnlyList<int> ShapeCount)
{
    public static Region Parse(string input)
    {
        var split = input.Split(':');
        var dimensions = split[0].Split('x');
        var width = int.Parse(dimensions[0]);
        var length = int.Parse(dimensions[1]);
        var shapeCount = split[1].Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        return new Region(width, length, shapeCount);
    }
}