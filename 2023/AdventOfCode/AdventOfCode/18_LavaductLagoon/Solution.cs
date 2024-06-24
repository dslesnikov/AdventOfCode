namespace AdventOfCode._18_LavaductLagoon;

public class Solution(IReadOnlyList<DigInstruction> digPlan) : ISolution<Solution>
{
    public static int Day => 18;

    public static Solution Parse(string s)
    {
        var digPlan = s.Split('\n')
            .Select(DigInstruction.Parse)
            .ToList();
        return new Solution(digPlan);
    }

    public string Part1()
    {
        var instructions = digPlan.Select(x => x.ExtractInstruction());
        var polygon = Polygon.FromInstructions(instructions);
        return polygon.GetArea().ToString();
    }

    public string Part2()
    {
        var instructions = digPlan.Select(x => x.DecodeInstruction());
        var polygon = Polygon.FromInstructions(instructions);
        return polygon.GetArea().ToString();
    }
}