namespace AdventOfCode.Day19LinenLayout;

public class Solution : IFromText<Solution>
{
    private readonly IReadOnlyList<string> _patterns;
    private readonly IReadOnlyList<string> _designs;

    private Solution(IReadOnlyList<string> patterns, IReadOnlyList<string> designs)
    {
        _patterns = patterns;
        _designs = designs;
    }

    public static int Day => 19;

    public string SolvePartOne()
    {
        var patterns = _patterns.ToHashSet();
        var cache = new Dictionary<string, long>();
        var result = _designs.Count(x => CountWays(x, patterns, cache) > 0);
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var patterns = _patterns.ToHashSet();
        var cache = new Dictionary<string, long>();
        var result = _designs.Sum(x => CountWays(x, patterns, cache));
        return result.ToString();
    }

    private long CountWays(
        ReadOnlySpan<char> design,
        HashSet<string> patterns,
        Dictionary<string, long> cache)
    {
        var alternateLookup = patterns.GetAlternateLookup<ReadOnlySpan<char>>();
        if (cache.GetAlternateLookup<ReadOnlySpan<char>>().TryGetValue(design, out var cached))
        {
            return cached;
        }

        var options = 0L;
        for (var prefixLength = 1; prefixLength < design.Length; prefixLength++)
        {
            var prefix = design.Slice(0, prefixLength);
            if (alternateLookup.Contains(prefix))
            {
                var suffix = design.Slice(prefixLength);
                var count = CountWays(suffix, patterns, cache);
                options += count;
            }
        }

        if (alternateLookup.Contains(design))
        {
            options++;
        }

        cache[design.ToString()] = options;
        return options;
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n");
        var patterns = split[0].Split(", ").ToArray();
        var designs = split[1].Split("\n").ToArray();
        return new Solution(patterns, designs);
    }
}