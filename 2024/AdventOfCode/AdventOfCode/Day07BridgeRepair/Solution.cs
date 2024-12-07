namespace AdventOfCode.Day07BridgeRepair;

public record Equation
{
    public required long Result { get; init; }

    public required IReadOnlyList<long> Operands { get; init; }

    public bool IsSimpleSolvable()
    {
        return IsSimpleSolvable(0, Operands[0]);
    }

    private bool IsSimpleSolvable(int index, long current)
    {
        if (index == Operands.Count - 1)
        {
            return current == Result;
        }

        var add = IsSimpleSolvable(index + 1, current + Operands[index + 1]);
        var multiply = IsSimpleSolvable(index + 1, current * Operands[index + 1]);
        return add || multiply;
    }
    public bool IsComplexSolvable()
    {
        return IsComplexSolvable(0, Operands[0]);
    }

    private bool IsComplexSolvable(int index, long current)
    {
        if (index == Operands.Count - 1)
        {
            return current == Result;
        }

        var add = IsComplexSolvable(index + 1, current + Operands[index + 1]);
        var multiply = IsComplexSolvable(index + 1, current * Operands[index + 1]);
        var log = Math.Log10(Operands[index + 1]);
        var concatValue = current * (long)Math.Pow(10, (int)log + 1) + Operands[index + 1];
        var concat = IsComplexSolvable(index + 1, concatValue);
        return add || multiply || concat;
    }

    public static Equation Parse(ReadOnlySpan<char> s)
    {
        var colonIndex = s.IndexOf(':');
        var result = long.Parse(s[..colonIndex]);
        var operandsSlice = s[(colonIndex + 2)..];
        var split = operandsSlice.Split(' ');
        var operands = new List<long>();
        foreach (var range in split)
        {
            operands.Add(long.Parse(operandsSlice[range]));
        }
        return new Equation
        {
            Result = result,
            Operands = operands
        };
    }
}

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