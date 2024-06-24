namespace AdventOfCode._10_PipeMaze;

public partial class Solution(IReadOnlyList<IReadOnlyList<char>> field) : ISolution<Solution>
{
    public static int Day => 10;

    public static Solution Parse(string s)
    {
        var field = s.Split('\n')
            .Select(line => line.ToCharArray())
            .ToArray();
        return new Solution(field);
    }
}