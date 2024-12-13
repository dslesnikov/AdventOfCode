namespace AdventOfCode.Day13ClawContraption;

public record Button(Point Move)
{
    public static Button FromString(ReadOnlySpan<char> line)
    {
        var point = Point.FromString(line);
        return new Button(point);
    }
}