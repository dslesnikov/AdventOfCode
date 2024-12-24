namespace AdventOfCode.Day24CrossedWires;

public enum Operation
{
    And,
    Or,
    Xor
}

public record Gate(string Input1, string Input2, Operation Operation, string Output)
{
    public bool Apply(Dictionary<string, bool?> wires)
    {
        var input1 = wires[Input1];
        var input2 = wires[Input2];
        if (input1 == null || input2 == null)
        {
            return false;
        }
        var output = Operation switch
        {
            Operation.And => input1 & input2,
            Operation.Or => input1 | input2,
            Operation.Xor => input1 ^ input2,
            _ => throw new InvalidOperationException()
        };
        wires[Output] = output;
        return true;
    }

    public static Gate FromText(string line)
    {
        var split = line.Split(' ');
        var input1 = split[0];
        var input2 = split[2];
        var output = split[4];
        var operation = split[1] switch
        {
            "AND" => Operation.And,
            "OR" => Operation.Or,
            "XOR" => Operation.Xor,
            _ => throw new InvalidOperationException()
        };
        return new Gate(input1, input2, operation, output);
    }
}

public class Solution : IFromText<Solution>
{
    public static int Day => 24;

    private readonly IReadOnlyDictionary<string, bool> _initialStates;
    private readonly IReadOnlyList<Gate> _gates;

    private Solution(IReadOnlyDictionary<string, bool> initialStates, IReadOnlyList<Gate> gates)
    {
        _initialStates = initialStates;
        _gates = gates;
    }

    public string SolvePartOne()
    {
        var allWires = _initialStates
            .Keys
            .Concat(_gates.SelectMany(x => new[] { x.Input1, x.Input2, x.Output }))
            .Distinct()
            .ToDictionary(x => x, x => _initialStates.TryGetValue(x, out var value) ? value : (bool?)null);
        var result = CalculateOutput(allWires);
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var mistakes = FindMistakes();
        return string.Join(',', mistakes.Order(StringComparer.Ordinal));
    }

    private HashSet<string> FindMistakes()
    {
        var mistakes = new HashSet<string>();
        foreach (var gate in _gates)
        {
            if (gate.Output.StartsWith('z') &&
                gate.Output != "z45" &&
                gate.Operation != Operation.Xor)
            {
                mistakes.Add(gate.Output);
            }

            if (!gate.Output.StartsWith('z') &&
                !(gate.Input1[0] is 'x' or 'y' && gate.Input2[0] is 'x' or 'y') &&
                gate.Operation is Operation.Xor)
            {
                mistakes.Add(gate.Output);
            }

            if (gate.Operation is Operation.Xor &&
                gate.Input1[0] is 'x' or 'y' &&
                gate.Input2[0] is 'x' or 'y' &&
                gate.Input1 != "x00" &&
                gate.Input2 != "x00")
            {
                var another = _gates.FirstOrDefault(x => x.Operation == Operation.Xor && (x.Input1 == gate.Output || x.Input2 == gate.Output));
                if (another is null)
                {
                    mistakes.Add(gate.Output);
                }
            }

            if (gate.Operation is Operation.And &&
                gate.Input1 != "x00" &&
                gate.Input2 != "x00")
            {
                var another = _gates.FirstOrDefault(x => x.Operation == Operation.Or && (x.Input1 == gate.Output || x.Input2 == gate.Output));
                if (another is null)
                {
                    mistakes.Add(gate.Output);
                }
            }
        }

        return mistakes;
    }

    private long CalculateOutput(Dictionary<string, bool?> allWires)
    {
        var remainingWires = allWires
            .Where(x => x.Key.StartsWith('z') && x.Value == null)
            .Select(x => x.Key)
            .ToHashSet();
        var iterations = 0;
        while (remainingWires.Count > 0)
        {
            foreach (var gate in _gates)
            {
                if (gate.Apply(allWires))
                {
                    remainingWires.Remove(gate.Output);
                }
            }
            iterations++;
            if (iterations > 100)
            {
                return long.MaxValue;
            }
        }
        var values = allWires
            .Where(x => x.Key.StartsWith('z'))
            .OrderBy(x => x.Key, StringComparer.Ordinal)
            .Select(x => x.Value)
            .ToArray();
        var result = 0L;
        for (var i = 0; i < values.Length; i++)
        {
            result |= values[i]!.Value ? 1L << i : 0;
        }

        return result;
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n");
        var initialStates = split[0]
            .Split('\n')
            .Select(static line =>
            {
                var split = line.Split(": ");
                var key = split[0];
                var value = split[1] == "1";
                return (key, value);
            })
            .ToDictionary(x => x.key, x => x.value);
        var gates = split[1]
            .Split('\n')
            .Select(Gate.FromText)
            .ToArray();
        return new Solution(initialStates, gates);
    }
}