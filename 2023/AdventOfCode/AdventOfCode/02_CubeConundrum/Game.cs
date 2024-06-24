using System.Text.RegularExpressions;

namespace AdventOfCode._02_CubeConundrum;

public partial record Game(int Id, IReadOnlyList<CubeSet> Sets) : ISimpleParsable<Game>
{
    [GeneratedRegex("^Game (?<groupId>\\d+): (?<sets>.*)$")]
    private static partial Regex LineExpression();

    public static Game Parse(string s)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(s);

        var match = LineExpression().Match(s);
        var groupId = int.Parse(match.Groups["groupId"].Value);
        var sets = match.Groups["sets"].Value.Split("; ")
            .Select(set => CubeSet.Parse(set, null))
            .ToArray();
        return new Game(groupId, sets);
    }
}