using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day17ChronospatialComputer;

public partial class Solution : IFromText<Solution>
{
    public static int Day => 17;

    private readonly int _a;
    private readonly int _b;
    private readonly int _c;
    private readonly ImmutableArray<byte> _opcodes;

    private Solution(int a, int b, int c, ImmutableArray<byte> opcodes)
    {
        _a = a;
        _b = b;
        _c = c;
        _opcodes = opcodes;
    }

    public string SolvePartOne()
    {
        var program = new Program(_a, _b, _c, _opcodes);
        var output = program.RunUntilCompletion();
        var result = string.Join(',', output);
        return result;
    }

    public string SolvePartTwo()
    {
        var queue = new Queue<(long Value, int TargetLength)>();
        for (var i = 1; i < 8; i++)
        {
            queue.Enqueue(((long)i << 45, 1));
        }

        Span<int> buffer = new int[_opcodes.Length];
        var result = long.MaxValue;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var program = new Program(current.Value, _b, _c, _opcodes);
            program.RunUntilCompletion(buffer);
            var isCorrect = true;
            for (var i = 0; i < current.TargetLength; i++)
            {
                var value = buffer[_opcodes.Length - 1 - i];
                var expected = _opcodes[_opcodes.Length - 1 - i];
                if (value != expected)
                {
                    isCorrect = false;
                    break;
                }
            }
            if (!isCorrect)
            {
                continue;
            }
            if (current.TargetLength == _opcodes.Length)
            {
                result = Math.Min(result, current.Value);
                continue;
            }

            for (var i = 0; i < 8; i++)
            {
                var newValue = current.Value | ((long)i << (45 - current.TargetLength * 3));
                queue.Enqueue((newValue, current.TargetLength + 1));
            }
        }

        return result.ToString();
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n");
        var state = split[0];
        var stateNumbers = Regexes.Number.EnumerateMatches(state);
        stateNumbers.MoveNext();
        var a = int.Parse(state.AsSpan(stateNumbers.Current.Index, stateNumbers.Current.Length));
        stateNumbers.MoveNext();
        var b = int.Parse(state.AsSpan(stateNumbers.Current.Index, stateNumbers.Current.Length));
        stateNumbers.MoveNext();
        var c = int.Parse(state.AsSpan(stateNumbers.Current.Index, stateNumbers.Current.Length));
        var opcodes = split[1].Split(' ')[1].Split(',').Select(byte.Parse);
        return new Solution(a, b, c, [..opcodes]);
    }

    private static partial class Regexes
    {
        [GeneratedRegex("\\d+")]
        public static partial Regex Number { get; }
    }
}
