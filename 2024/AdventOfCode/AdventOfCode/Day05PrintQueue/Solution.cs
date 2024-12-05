namespace AdventOfCode.Day05PrintQueue;

public class Solution : IFromText<Solution>
{
    public static int Day => 5;

    private readonly IReadOnlyDictionary<int, IReadOnlySet<int>> _preceding;
    private readonly IReadOnlyList<IReadOnlyList<int>> _updates;

    private Solution(
        IReadOnlyDictionary<int, IReadOnlySet<int>> preceding,
        IReadOnlyList<IReadOnlyList<int>> updates)
    {
        _preceding = preceding;
        _updates = updates;
    }

    public string SolvePartOne()
    {
        var result = 0;
        foreach (var update in _updates)
        {
            var orderIsCorrect = true;
            for (var i = 0; i < update.Count; i++)
            {
                for (var j = i + 1; j < update.Count; j++)
                {
                    if (_preceding.TryGetValue(update[i], out var beforeValues) &&
                        beforeValues.Contains(update[j]))
                    {
                        orderIsCorrect = false;
                        break;
                    }
                }
                if (!orderIsCorrect)
                {
                    break;
                }
            }
            if (orderIsCorrect)
            {
                result += update[update.Count / 2];
            }
        }
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var result = 0;
        foreach (var update in _updates)
        {
            var ordered = update.ToArray();
            var orderIsCorrect = true;
            for (var i = 0; i < ordered.Length; i++)
            {
                for (var j = 0; j < ordered.Length - i - 1; j++)
                {
                    if (_preceding.TryGetValue(ordered[j], out var beforeValues) &&
                        beforeValues.Contains(ordered[j + 1]))
                    {
                        orderIsCorrect = false;
                        (ordered[j], ordered[j + 1]) = (ordered[j + 1], ordered[j]);
                    }
                }
            }
            if (!orderIsCorrect)
            {
                result += ordered[ordered.Length / 2];
            }
        }
        return result.ToString();
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n");
        var preceding = split[0]
            .Split('\n')
            .Select(line =>
            {
                var parts = line.Split('|');
                return (Before: int.Parse(parts[0]), After: int.Parse(parts[1]));
            })
            .GroupBy(x => x.After, pair => pair.Before)
            .ToDictionary(x => x.Key, IReadOnlySet<int> (x) => x.ToHashSet());
        var updates = split[1]
            .Split('\n')
            .Select(IReadOnlyList<int> (line) => line.Split(',').Select(int.Parse).ToArray())
            .ToArray();
        return new Solution(preceding, updates);
    }
}
