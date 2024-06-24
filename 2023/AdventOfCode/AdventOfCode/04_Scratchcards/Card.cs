using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode._04_Scratchcards;

public partial record Card(int Id, ImmutableHashSet<int> WinningNumbers, ImmutableHashSet<int> MyNumbers) : ISimpleParsable<Card>
{
    [GeneratedRegex(@"^Card\s+(?<id>\d+):(\s+(?<winningNumber>\d+))+\s*\|(\s+(?<myNumber>\d+))+$")]
    private static partial Regex GamePattern();

    public static Card Parse(string s)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(s);
        var match = GamePattern().Match(s);
        var id = int.Parse(match.Groups["id"].Value);
        var winningNumbers = match.Groups["winningNumber"]
            .Captures
            .Select(x => int.Parse(x.Value))
            .ToImmutableHashSet();
        var myNumbers = match.Groups["myNumber"]
            .Captures
            .Select(x => int.Parse(x.Value))
            .ToImmutableHashSet();
        return new Card(id, winningNumbers, myNumbers);
    }
}