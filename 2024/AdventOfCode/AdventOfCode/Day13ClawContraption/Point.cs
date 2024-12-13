using System.Text.RegularExpressions;

namespace AdventOfCode.Day13ClawContraption;

public readonly partial record struct Point(long X, long Y)
{
    public static Point FromString(ReadOnlySpan<char> line)
    {
        var matches = Number.EnumerateMatches(line);
        matches.MoveNext();
        var x = long.Parse(line.Slice(matches.Current.Index, matches.Current.Length));
        matches.MoveNext();
        var y = long.Parse(line.Slice(matches.Current.Index, matches.Current.Length));
        return new Point(x, y);
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex Number { get; }
}