namespace AdventOfCode.Day14RestroomRedoubt;

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