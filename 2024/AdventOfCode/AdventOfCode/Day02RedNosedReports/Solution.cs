namespace AdventOfCode.Day02RedNosedReports;

public class Solution : IFromLines<Solution, IReadOnlyList<int>>
{
    public static int Day => 2;

    private readonly IReadOnlyList<IReadOnlyList<int>> _reports;

    private Solution(IReadOnlyList<IReadOnlyList<int>> reports)
    {
        _reports = reports;
    }

    public string SolvePartOne()
    {
        var safeCount = 0;
        foreach (var report in _reports)
        {
            var sign = Math.Sign(report[1] - report[0]);
            if (sign == 0)
            {
                continue;
            }

            var isSafe = true;
            for (var i = 1; i < report.Count; i++)
            {
                isSafe &= Math.Sign(report[i] - report[i - 1]) == sign &&
                          Math.Abs(report[i] - report[i - 1]) <= 3 &&
                          Math.Abs(report[i] - report[i - 1]) >= 1;
            }
            if (isSafe)
            {
                safeCount++;
            }
        }
        return safeCount.ToString();
    }

    public string SolvePartTwo()
    {
        var safeCount = 0;
        foreach (var report in _reports)
        {
            var anyVariantIsSafe = false;
            for (var omit = 0; omit < report.Count; omit++)
            {
                var sign = omit <= 1
                    ? Math.Sign(report[^1] - report[^2])
                    : Math.Sign(report[1] - report[0]);
                var isSafe = true;
                for (var i = 1; i < report.Count; i++)
                {
                    if (i == omit || (i == 1 && omit == 0))
                    {
                        continue;
                    }

                    var current = report[i];
                    var previous = omit == i - 1 ? report[i - 2] : report[i - 1];
                    isSafe &= Math.Sign(current - previous) == sign &&
                              Math.Abs(current - previous) <= 3 &&
                              Math.Abs(current - previous) >= 1;
                }
                if (isSafe)
                {
                    anyVariantIsSafe = true;
                    break;
                }
            }
            if (anyVariantIsSafe)
            {
                safeCount++;
            }
        }
        return safeCount.ToString();
    }

    public static IReadOnlyList<int> ParseLine(ReadOnlySpan<char> line)
    {
        var values = new List<int>();
        var numbers = line.Split(' ');
        foreach (var numberRange in numbers)
        {
            var value = int.Parse(line[numberRange]);
            values.Add(value);
        }
        return values;
    }

    public static Solution FromParsed(IReadOnlyList<IReadOnlyList<int>> entries)
    {
        return new Solution(entries);
    }
}