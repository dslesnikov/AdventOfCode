namespace AdventOfCode._15_LensLibrary;

public class Solution(IReadOnlyList<Operation> steps) : ISolution<Solution>
{
    public static int Day => 15;

    public static Solution Parse(string s)
    {
        var steps = s.Split(',').Select(Operation.Parse).ToArray();
        return new Solution(steps);
    }

    public string Part1()
    {
        return steps.Sum(s => CalculateHash(s.Instruction)).ToString();
    }

    public string Part2()
    {
        var boxes = new Box[256];
        for (var i = 0; i < 256; i++)
        {
            boxes[i] = new Box();
        }
        foreach (var step in steps)
        {
            boxes[CalculateHash(step.Label)].Execute(step);
        }
        var focusingPower = 0;
        for (var i = 0; i < 256; i++)
        {
            var box = boxes[i];
            var slot = 1;
            foreach (var lens in box)
            {
                checked
                {
                    focusingPower += (i + 1) * slot++ * lens;
                }
            }
        }
        return focusingPower.ToString();
    }

    private int CalculateHash(string input)
    {
        var current = 0;
        foreach (var c in input)
        {
            current += c;
            current *= 17;
            current %= 256;
        }
        return current;
    }
}