namespace AdventOfCode._22_SandSlabs;

public record BrickSlab(Point From, Point To) : ISimpleParsable<BrickSlab>
{
    public static BrickSlab Parse(string s)
    {
        var split = s.Split('~');
        return new BrickSlab(Point.Parse(split[0]), Point.Parse(split[1]));
    }
}