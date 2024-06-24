namespace AdventOfCode._24_NeverTellMeTheOdds;

public record HailStone(Point Position, Point Velocity) : ISimpleParsable<HailStone>
{
    public static HailStone Parse(string s)
    {
        var parts = s.Split(" @ ");
        var position = Point.Parse(parts[0]);
        var velocity = Point.Parse(parts[1]);
        return new HailStone(position, velocity);
    }

    public PlainIntersectionResult? GetPlainIntersection(HailStone another)
    {
        var k = (decimal)Velocity.Y / Velocity.X;
        var b = Position.Y - k * Position.X;
        var anotherK = (decimal)another.Velocity.Y / another.Velocity.X;
        var anotherB = another.Position.Y - anotherK * another.Position.X;
        if (k == anotherK && b != anotherB)
        {
            return default;
        }
        var intersectionX = (b - anotherB) / (anotherK - k);
        var intersectionY = k * intersectionX + b;
        var ourTime = (intersectionX - Position.X) / Velocity.X;
        var anotherTime = (intersectionX - another.Position.X) / another.Velocity.X;
        return new PlainIntersectionResult(ourTime, anotherTime, intersectionX, intersectionY);
    }
}