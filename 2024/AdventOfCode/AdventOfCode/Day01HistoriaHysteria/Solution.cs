namespace AdventOfCode.Day01HistoriaHysteria;

public class Solution : IFromLines<Solution, (int, int)>
{
    public static int Day => 1;

    private readonly IReadOnlyList<int> _firstColumn;
    private readonly IReadOnlyList<int> _secondColumn;

    private Solution(IReadOnlyList<int> firstColumn, IReadOnlyList<int> secondColumn)
    {
        _firstColumn = firstColumn;
        _secondColumn = secondColumn;
    }

    public string SolvePartOne()
    {
        var sortedFirstColumn = _firstColumn.Order().ToArray();
        var sortedSecondColumn = _secondColumn.Order().ToArray();
        var result = sortedFirstColumn.Zip(sortedSecondColumn, (a, b) => Math.Abs(a - b)).Sum();
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var secondSet = _secondColumn
            .CountBy(x => x)
            .ToDictionary(x => x.Key, x => x.Value);
        var result = 0;
        foreach (var value in _firstColumn)
        {
            secondSet.TryGetValue(value, out var count);
            result += count * value;
        }

        return result.ToString();
    }

    public static (int, int) ParseLine(ReadOnlySpan<char> line)
    {
        var split = line.Split("   ");
        split.MoveNext();
        var first = int.Parse(line[split.Current.Start..split.Current.End]);
        split.MoveNext();
        var second = int.Parse(line[split.Current.Start..split.Current.End]);
        return (first, second);
    }

    public static Solution FromParsed(IReadOnlyList<(int, int)> entries)
    {
        var firstColumn = entries.Select(x => x.Item1).ToList();
        var secondColumn = entries.Select(x => x.Item2).ToList();
        return new Solution(firstColumn, secondColumn);
    }
}