namespace AdventOfCode.Day10Factory;

public class Solution : ISolution, IFromLines<Solution, Machine>
{
    public static int Day => 10;

    private readonly IReadOnlyList<Machine> _machines;

    private Solution(IReadOnlyList<Machine> machines)
    {
        _machines = machines;
    }

    public string SolvePartOne()
    {
        var result = 0L;
        foreach (var machine in _machines)
        {
            result += machine.SolveForLights();
        }
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var result = 0L;
        foreach (var machine in _machines)
        {
            result += machine.SolveForJoltages();
        }
        return result.ToString();
    }

    public static Machine ParseLine(string line)
    {
        return Machine.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<Machine> entries)
    {
        return new Solution(entries);
    }
}