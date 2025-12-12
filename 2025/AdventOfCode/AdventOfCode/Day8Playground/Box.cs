namespace AdventOfCode.Day8Playground;

public record Box(int X, int Y, int Z)
{
    public double Distance(Box another)
    {
        return Math.Sqrt(Math.Pow(X - another.X, 2) + Math.Pow(Y - another.Y, 2) + Math.Pow(Z - another.Z, 2));
    }

    public static Box Parse(string s)
    {
        var split = s.Split(',');
        return new Box(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
    }
}