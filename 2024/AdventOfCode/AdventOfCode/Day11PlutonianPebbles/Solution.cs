namespace AdventOfCode.Day11PlutonianPebbles;

public class Solution : IFromText<Solution>
{
    public static int Day => 11;

    private readonly IReadOnlyList<int> _stones;

    private Solution(IReadOnlyList<int> stones)
    {
        _stones = stones;
    }

    public string SolvePartOne()
    {
        var result = _stones.Sum(x => Blink(25, x, new()));
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var result = _stones.Sum(x => Blink(75, x, new()));
        return result.ToString();
    }

    private static long Blink(int times, long value, Dictionary<(int, long), long> cache)
    {
        if (cache.TryGetValue((times, value), out var cached))
        {
            return cached;
        }
        if (times == 0)
        {
            return 1;
        }

        if (value == 0)
        {
            var result = Blink(times - 1, 1, cache);
            cache[(times, value)] = result;
            return result;
        }
        var log = (int)Math.Log10(value);
        if (log % 2 == 1)
        {
            var dividerPower = log / 2 + 1;
            var divider = (int)Math.Pow(10, dividerPower);
            var right = value % divider;
            var left = value / divider;
            var result = Blink(times - 1, right, cache) + Blink(times - 1, left, cache);
            cache[(times, value)] = result;
            return result;
        }
        else
        {
            var result = Blink(times - 1, value * 2024, cache);
            cache[(times, value)] = result;
            return result;
        }
    }

    public static Solution FromText(string text)
    {
        return new Solution(text.Split(' ').Select(int.Parse).ToArray());
    }
}