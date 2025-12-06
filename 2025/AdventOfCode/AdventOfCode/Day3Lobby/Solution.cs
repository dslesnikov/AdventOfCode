namespace AdventOfCode.Day3Lobby;

public class Solution : ISolution, IFromLines<Solution, BatteryBank>
{
    public static int Day => 3;

    private readonly IReadOnlyList<BatteryBank> _batteryBanks;

    private Solution(IReadOnlyList<BatteryBank> batteryBanks)
    {
        _batteryBanks = batteryBanks;
    }

    public string SolvePartOne()
    {
        var totalJoltage = 0;
        foreach (var bank in _batteryBanks)
        {
            totalJoltage += (int)GetMaxJoltage(bank.Values, 2);
        }

        return totalJoltage.ToString();
    }

    public string SolvePartTwo()
    {
        var totalJoltage = 0L;
        foreach (var bank in _batteryBanks)
        {
            totalJoltage += GetMaxJoltage(bank.Values, 12);
        }
        return totalJoltage.ToString();
    }

    private long GetMaxJoltage(IReadOnlyList<int> values, int targetLength)
    {
        Span<int> current = stackalloc int[targetLength];
        for (var i = 0; i < targetLength; i++)
        {
            current[i] = values[i];
        }
        for (var i = targetLength; i < values.Count; i++)
        {
            var next = values[i];
            for (var j = 0; j < targetLength - 1; j++)
            {
                if (current[j] < current[j + 1])
                {
                    current[j] = current[j + 1];
                    current[j + 1] = -1;
                }
            }
            current[^1] = Math.Max(current[^1], next);
        }
        var value = 0L;
        foreach (var v in current)
        {
            value = value * 10 + v;
        }
        return value;
    }

    public static BatteryBank ParseLine(string line)
    {
        return BatteryBank.FromString(line);
    }

    public static Solution FromParsed(IReadOnlyList<BatteryBank> entries)
    {
        return new Solution(entries);
    }
}
