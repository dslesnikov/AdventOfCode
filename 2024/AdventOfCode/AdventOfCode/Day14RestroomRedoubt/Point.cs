namespace AdventOfCode.Day14RestroomRedoubt;

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