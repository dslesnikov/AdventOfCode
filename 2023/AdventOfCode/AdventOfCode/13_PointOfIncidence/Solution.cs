namespace AdventOfCode._13_PointOfIncidence;

public class Solution(IReadOnlyList<Pattern> patterns) : ISolution<Solution>
{
    public static int Day => 13;

    public static Solution Parse(string s)
    {
        var patterns = s.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(Pattern.Parse)
            .ToArray();
        return new Solution(patterns);
    }

    public string Part1()
    {
        var result = patterns
            .Select(p => p.FindPerfectReflectionPoint())
            .Sum(p => p.Summarize());
        return result.ToString();
    }

    public string Part2()
    {
        var result = patterns
            .Select(p => p.FindSingleSmudgeReflectionPoint())
            .Sum(p => p.Summarize());
        return result.ToString();
    }
}