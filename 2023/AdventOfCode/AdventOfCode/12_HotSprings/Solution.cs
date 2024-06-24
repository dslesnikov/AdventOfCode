namespace AdventOfCode._12_HotSprings;

public class Solution(IReadOnlyList<ConditionRecord> records) : ISolution<Solution>
{
    public static int Day => 12;

    public static Solution Parse(string s)
    {
        var records = s.Split('\n')
            .Select(ConditionRecord.Parse)
            .ToArray();
        return new Solution(records);
    }

    public string Part1()
    {
        var arrangements = records.Select(r => r.CountValidArrangements());
        return arrangements.Sum().ToString();
    }

    public string Part2()
    {
        var newRecords = records.Select(r => r.Unfold());
        var arrangements = newRecords.Select(r => r.CountValidArrangements());
        return arrangements.Sum().ToString();
    }
}