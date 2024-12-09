namespace AdventOfCode.Day07BridgeRepair;

public class Solution : IFromLines<Solution, Equation>
{
    public static int Day => 7;

    private readonly IReadOnlyList<Equation> _equations;

    private Solution(IReadOnlyList<Equation> equations)
    {
        _equations = equations;
    }

    public string SolvePartOne()
    {
        var result = 0L;
        foreach (var equation in _equations)
        {
            if (equation.IsSimpleSolvable())
            {
                result += equation.Result;
            }
        }
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var result = 0L;
        foreach (var equation in _equations)
        {
            if (equation.IsComplexSolvable())
            {
                result += equation.Result;
            }
        }
        return result.ToString();
    }

    public static Equation ParseLine(ReadOnlySpan<char> line)
    {
        return Equation.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<Equation> entries)
    {
        return new Solution(entries);
    }
}