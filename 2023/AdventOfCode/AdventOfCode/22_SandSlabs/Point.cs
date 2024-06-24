namespace AdventOfCode._22_SandSlabs;

public readonly record struct Point(int X, int Y, int Z) : ISimpleParsable<Point>
{
    public static Point Parse(string s)
    {
        var split = s.Split(',');
        return new Point(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
    }
}