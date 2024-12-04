using System.Text.RegularExpressions;

namespace AdventOfCode.Day03MullItOver;

public partial class Solution : IFromText<Solution>
{
    public static int Day => 3;

    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MultiplyInstructionRegex { get; }
    [GeneratedRegex(@"do\(\)")]
    private static partial Regex DoInstructionRegex { get; }

    private readonly string _input;

    private Solution(string input)
    {
        _input = input;
    }

    public string SolvePartOne()
    {
        var result = SumMultInstructions(_input);
        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var doSegments = DoInstructionRegex.EnumerateSplits(_input);
        var result = 0;
        foreach (var segment in doSegments)
        {
            var slice = _input.AsSpan(segment);
            var dont = slice.IndexOf("don't()");
            slice = dont == -1 ? slice : slice[..dont];
            result += SumMultInstructions(slice);
        }
        return result.ToString();
    }

    private static int SumMultInstructions(ReadOnlySpan<char> slice)
    {
        var matches = MultiplyInstructionRegex.EnumerateMatches(slice);
        var result = 0;
        foreach (var multSegment in matches)
        {
            var mulValue = slice.Slice(multSegment.Index, multSegment.Length);
            var comma = mulValue.IndexOf(',');
            var left = int.Parse(mulValue[4..comma]);
            var right = int.Parse(mulValue[(comma + 1)..^1]);
            result += left * right;
        }
        return result;
    }

    public static Solution FromText(string text)
    {
        return new Solution(text);
    }
}