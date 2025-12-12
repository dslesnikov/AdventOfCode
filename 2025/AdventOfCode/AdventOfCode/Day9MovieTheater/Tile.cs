namespace AdventOfCode.Day9MovieTheater;

public readonly record struct Tile(long X, long Y)
{
    public long Area(Tile another)
    {
        return (Math.Abs(X - another.X) + 1) * (Math.Abs(Y - another.Y) + 1);
    }
    
    public static Tile Parse(string input)
    {
        var parts = input.Split(',');
        return new Tile(long.Parse(parts[0]), long.Parse(parts[1]));
    }
}