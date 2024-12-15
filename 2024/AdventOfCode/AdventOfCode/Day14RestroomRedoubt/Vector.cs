namespace AdventOfCode.Day14RestroomRedoubt;

public record Vector(int X, int Y)
{
    public static Vector FromString(ReadOnlySpan<char> line)
    {
        var numberMatches = Regexes.Number.EnumerateMatches(line);
        numberMatches.MoveNext();
        var x = int.Parse(line.Slice(numberMatches.Current.Index, numberMatches.Current.Length));
        numberMatches.MoveNext();
        var y = int.Parse(line.Slice(numberMatches.Current.Index, numberMatches.Current.Length));
        return new Vector(x, y);
    }
}