namespace AdventOfCode.Day13ClawContraption;

public class Solution : IFromText<Solution>
{
    public static int Day => 13;

    private readonly IReadOnlyList<ClawMachine> _machines;

    private Solution(IReadOnlyList<ClawMachine> machines)
    {
        _machines = machines;
    }

    public string SolvePartOne()
    {
        var result = _machines.Sum(x => x.Solve(100));
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var converted = _machines
            .Select(x => x with
            {
                Prize = new Point(
                    X: x.Prize.X + 10000000000000,
                    Y: x.Prize.Y + 10000000000000)
            })
            .Sum(x => x.Solve(long.MaxValue));
        return converted.ToString();
    }

    public static Solution FromText(string text)
    {
        var machines = text
            .Split("\n\n")
            .Select(x => ClawMachine.FromString(x))
            .ToArray();
        return new Solution(machines);
    }
}