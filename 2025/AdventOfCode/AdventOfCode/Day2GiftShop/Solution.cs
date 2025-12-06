namespace AdventOfCode.Day2GiftShop;

public class Solution : ISolution, IFromText<Solution>
{
    public static int Day => 2;

    private readonly IReadOnlyList<IdRange> _ranges;

    private Solution(IReadOnlyList<IdRange> ranges)
    {
        _ranges = ranges;
    }

    public string SolvePartOne()
    {
        var invalidIdSum = 0L;
        foreach (var range in _ranges)
        {
            for (var id = range.First; id <= range.Last; id++)
            {
                var str = id.ToString();
                if (str.Length % 2 == 0 &&
                    str[..(str.Length / 2)] == str[(str.Length / 2)..])
                {
                    invalidIdSum += id;
                }
            }
        }
        return invalidIdSum.ToString();
    }

    public string SolvePartTwo()
    {
        var invalidIdSum = 0L;
        foreach (var range in _ranges)
        {
            for (var id = range.First; id <= range.Last; id++)
            {
                var str = id.ToString();
                for (var length = 1; length <= str.Length / 2; length++)
                {
                    if (str.Length % length != 0)
                    {
                        continue;
                    }
                    var consistsOfRepeats = true;
                    for (var i = length; i < str.Length; i += length)
                    {
                        if (str[i..(i + length)] != str[..length])
                        {
                            consistsOfRepeats = false;
                            break;
                        }
                    }
                    if (consistsOfRepeats)
                    {
                        invalidIdSum += id;
                        break;
                    }
                }
            }
        }
        return invalidIdSum.ToString();
    }

    public static Solution FromText(string text)
    {
        var ranges = text.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(IdRange.Parse)
            .ToList();
        return new Solution(ranges);
    }
}
