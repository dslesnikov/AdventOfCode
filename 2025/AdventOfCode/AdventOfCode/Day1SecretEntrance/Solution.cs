namespace AdventOfCode.Day1SecretEntrance;

public class Solution : IFromLines<Solution, Rotation>, ISolution
{
    private readonly IReadOnlyList<Rotation> _rotations;

    private Solution(IReadOnlyList<Rotation> rotations)
    {
        _rotations = rotations;
    }

    public static int Day => 1;

    public string SolvePartOne()
    {
        var current = 50;
        var zeroCount = 0;
        foreach (var rotation in _rotations)
        {
            current = rotation.Apply(current);
            if (current == 0)
            {
                zeroCount++;
            }
        }
        return zeroCount.ToString();
    }

    public string SolvePartTwo()
    {
        var current = 50;
        var zeroCount = 0;
        foreach (var rotation in _rotations)
        {
            (current, var currentZeros) = rotation.ApplyCountingZeros(current);
            zeroCount += currentZeros;
        }
        return zeroCount.ToString();
    }

    public static Rotation ParseLine(string line)
    {
        return Rotation.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<Rotation> entries)
    {
        return new Solution(entries);
    }
}
