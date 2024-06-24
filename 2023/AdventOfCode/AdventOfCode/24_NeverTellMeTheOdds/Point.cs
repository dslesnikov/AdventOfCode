namespace AdventOfCode._24_NeverTellMeTheOdds;

public readonly record struct Point(long X, long Y, long Z) : ISimpleParsable<Point>
{
    public static Point Parse(string s)
    {
        var parts = s.Split(", ");
        return new Point(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
    }

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
}