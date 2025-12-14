using Microsoft.Z3;

namespace AdventOfCode.Day10Factory;

public class Machine
{
    private readonly int _targetLight;
    private readonly int[][] _buttons;
    private readonly int[] _joltages;

    private Machine(int targetLight, int[][] buttons, int[] joltages)
    {
        _targetLight = targetLight;
        _buttons = buttons;
        _joltages = joltages;
    }

    public int SolveForLights()
    {
        const int currentLight = 0;
        var queue = new Queue<(int Current, int Length)>();
        var visited = new HashSet<int> { currentLight };
        queue.Enqueue((currentLight, 0));
        while (queue.TryDequeue(out var current))
        {
            if (current.Current == _targetLight)
            {
                return current.Length;
            }

            foreach (var buttonLights in _buttons)
            {
                var nextLight = current.Current;
                foreach (var index in buttonLights)
                {
                    nextLight ^= 1 << index;
                }
                if (visited.Add(nextLight))
                {
                    queue.Enqueue((nextLight, current.Length + 1));
                }
            }
        }
        return -1;
    }

    public long SolveForJoltages()
    {
        var context = new Context();
        var optimize = context.MkOptimize();
        var buttonPresses = new IntExpr[_buttons.Length];
        for (var i = 0; i < _buttons.Length; i++)
        {
            buttonPresses[i] = context.MkIntConst($"button{i}");
            var zeroOrMore = context.MkGe(buttonPresses[i], context.MkInt(0));
            optimize.Assert(zeroOrMore);
        }
        var totalPressesExpr = context.MkAdd(buttonPresses.Cast<ArithExpr>());
        var minimizeHandle = optimize.MkMinimize(totalPressesExpr);

        for (var i = 0; i < _joltages.Length; i++)
        {
            var buttonIndexes = _buttons.Select((item, index) => (item, index))
                .Where(pair => pair.item.Contains(i))
                .Select(pair => pair.index)
                .ToArray();
            var buttons = buttonIndexes
                .Select(ArithExpr (index) => buttonPresses[index])
                .ToArray();
            var sum = context.MkAdd(buttons);
            var targetValue = context.MkInt(_joltages[i]);
            var equality = context.MkEq(sum, targetValue);
            optimize.Assert(equality);
        }
        var result = optimize.Check();
        if (result != Status.SATISFIABLE)
        {
            return -1;
        }

        var model = optimize.Model;
        var totalPresses = model.Eval(minimizeHandle.Lower);
        return (totalPresses as IntNum)!.Int64;
    }

    public static Machine Parse(string input)
    {
        var split = input.Split(' ');
        var lights = split[0][1..^1];
        var targetLight = 0;
        for (var i = 0; i < lights.Length; i++)
        {
            if (lights[i] == '#')
            {
                targetLight |= 1 << i;
            }
        }

        var buttons = split[1..^1]
            .Select(button => button[1..^1].Split(',').Select(int.Parse).ToArray())
            .ToArray();
        var joltages = split[^1][1..^1].Split(',').Select(int.Parse).ToArray();
        return new Machine(targetLight, buttons, joltages);
    }
}