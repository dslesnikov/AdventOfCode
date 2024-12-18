using System.Collections.Immutable;

namespace AdventOfCode.Day17ChronospatialComputer;

public ref struct Program
{
    private long _a;
    private long _b;
    private long _c;
    private int _instructionPointer;
    private readonly ImmutableArray<byte> _opcodes;

    public Program(long a, long b, long c, ImmutableArray<byte> opcodes)
    {
        _a = a;
        _b = b;
        _c = c;
        _opcodes = opcodes;
    }

    public IReadOnlyList<int> RunUntilCompletion()
    {
        var result = new List<int>();
        while (_instructionPointer < _opcodes.Length - 1)
        {
            var iteration = ExecuteIteration();
            if (iteration is not null)
            {
                result.Add(iteration.Value);
            }
        }
        return result;
    }

    public int RunUntilCompletion(in Span<int> outputBuffer)
    {
        var index = 0;
        while (_instructionPointer < _opcodes.Length - 1)
        {
            var iteration = ExecuteIteration();
            if (iteration is not null)
            {
                outputBuffer[index++] = iteration.Value;
            }
        }
        return index;
    }

    private int? ExecuteIteration()
    {
        var instruction = _opcodes[_instructionPointer];
        var operand = _opcodes[_instructionPointer + 1];
        switch (instruction)
        {
            case 0:
            {
                var numerator = _a;
                var denominator = Math.Pow(2, Combo(operand));
                var result = (long)Math.Truncate(numerator / denominator);
                _a = result;
                _instructionPointer += 2;
                break;
            }
            case 1:
            {
                _b ^= operand;
                _instructionPointer += 2;
                break;
            }
            case 2:
            {
                _b = Combo(operand) % 8;
                _instructionPointer += 2;
                break;
            }
            case 3:
            {
                if (_a == 0)
                {
                    _instructionPointer += 2;
                    break;
                }
                _instructionPointer = operand;
                break;
            }
            case 4:
            {
                _b ^= _c;
                _instructionPointer += 2;
                break;
            }
            case 5:
            {
                _instructionPointer += 2;
                return (int)(Combo(operand) % 8);
            }
            case 6:
            {
                var numerator = _a;
                var denominator = Math.Pow(2, Combo(operand));
                var result = (long)Math.Truncate(numerator / denominator);
                _b = result;
                _instructionPointer += 2;
                break;
            }
            case 7:
            {
                var numerator = _a;
                var denominator = Math.Pow(2, Combo(operand));
                var result = (long)Math.Truncate(numerator / denominator);
                _c = result;
                _instructionPointer += 2;
                break;
            }
        }
        return null;
    }

    private long Combo(int value)
    {
        return value switch
        {
            >= 0 and <= 3 => value,
            4 => _a,
            5 => _b,
            6 => _c,
            _ => throw new NotSupportedException()
        };
    }
}