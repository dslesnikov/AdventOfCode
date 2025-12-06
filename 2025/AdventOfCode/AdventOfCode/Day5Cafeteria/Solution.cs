namespace AdventOfCode.Day5Cafeteria;

public record IdRange(long Min, long Max)
{
    public static IdRange Parse(string input)
    {
        var parts = input.Split('-');
        return new IdRange(long.Parse(parts[0]), long.Parse(parts[1]));
    }
}

public class Solution : ISolution, IFromText<Solution>
{
    public static int Day => 5;

    private readonly IReadOnlyList<IdRange> _freshRanges;
    private readonly IReadOnlyList<long> _ids;


    private Solution(IReadOnlyList<IdRange> freshRanges, IReadOnlyList<long> ids)
    {
        _freshRanges = freshRanges;
        _ids = ids;
    }

    public string SolvePartOne()
    {
        var freshCount = 0;
        foreach (var id in _ids)
        {
            var isFresh = _freshRanges.Any(x => id >= x.Min && id <= x.Max);
            if (isFresh)
            {
                freshCount++;
            }
        }
        return freshCount.ToString();
    }

    public string SolvePartTwo()
    {
        var nonOverlappingRanges = new List<IdRange>();
        var sortedRanges = _freshRanges
            .OrderBy(x => x.Min)
            .ThenBy(x => x.Max)
            .ToList();
        foreach (var range in sortedRanges)
        {
            if (nonOverlappingRanges.Count == 0)
            {
                nonOverlappingRanges.Add(range);
                continue;
            }
            var lastRange = nonOverlappingRanges[^1];
            if (range.Min > lastRange.Max)
            {
                nonOverlappingRanges.Add(range);
            }
            else
            {
                var mergedRange = new IdRange(
                    Math.Min(lastRange.Min, range.Min),
                    Math.Max(lastRange.Max, range.Max));
                nonOverlappingRanges[^1] = mergedRange;
            }
        }

        var result = nonOverlappingRanges.Sum(x => x.Max - x.Min + 1);

        return result.ToString();
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var ranges = split[0]
            .Split('\n')
            .Select(IdRange.Parse)
            .ToArray();
        var ids = split[1]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();
        return new Solution(ranges, ids);
    }
}
