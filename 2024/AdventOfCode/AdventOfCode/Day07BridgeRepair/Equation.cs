namespace AdventOfCode.Day07BridgeRepair;

public record Equation
{
    public required long Result { get; init; }

    public required IReadOnlyList<long> Operands { get; init; }

    public bool IsSimpleSolvable()
    {
        return IsSolvable(0, Operands[0],
            (a, b) => a + b,
            (a, b) => a * b);
    }

    public bool IsComplexSolvable()
    {
        return IsSolvable(0, Operands[0],
            (a, b) => a + b,
            (a, b) => a * b,
            (a, b) => a * (long)Math.Pow(10, (int)Math.Log10(b) + 1) + b);
    }

    private bool IsSolvable(int index, long current,
        params ReadOnlySpan<Func<long, long, long>> operations)
    {
        if (index == Operands.Count - 1)
        {
            return current == Result;
        }

        if (current > Result)
        {
            return false;
        }

        foreach (var operation in operations)
        {
            var next = operation(current, Operands[index + 1]);
            if (IsSolvable(index + 1, next, operations))
            {
                return true;
            }
        }
        return false;
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