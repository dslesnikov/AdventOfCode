namespace AdventOfCode.Day6TrashCompactor;

public class Solution : ISolution, IFromText<Solution>
{
    public static int Day => 6;

    private readonly string _text;

    private Solution(string text)
    {
        _text = text;
    }

    public string SolvePartOne()
    {
        var lines = _text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var numbers = lines.Take(lines.Length - 1)
            .Select(IReadOnlyList<int> (line) => line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToList())
            .ToList();
        var operationLine = lines[^1];
        var operations = operationLine
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(opChar => opChar switch
            {
                "+" => Operation.Add,
                "*" => Operation.Multiply,
                _ => throw new InvalidOperationException($"Unknown operation character: {opChar}")
            })
            .ToList();
        var total = 0L;
        for (var i = 0; i < operations.Count; i++)
        {
            var operation = operations[i];
            var operands = numbers.Select(row => row[i]).ToList();
            var result = operation switch
            {
                Operation.Add => operands.Sum(x => (long)x),
                Operation.Multiply => operands.Aggregate(1L, (acc, x) => acc * x),
                _ => throw new InvalidOperationException($"Unknown operation: {operation}")
            };
            total += result;
        }
        return total.ToString();
    }

    public string SolvePartTwo()
    {
        var lines = _text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var total = 0L;
        var lastSeparator = -1;
        while (lastSeparator != lines[0].Length)
        {
            var currentSeparator = lines[0].Length;
            for (var i = lastSeparator + 1; i < lines[0].Length; i++)
            {
                if (lines.All(x => x[i] == ' '))
                {
                    currentSeparator = i;
                    break;
                }
            }

            var numbers = new List<long>();
            for (var i = currentSeparator - 1; i > lastSeparator; i--)
            {
                var number = lines
                    .Select(line => line[i])
                    .Where(char.IsDigit)
                    .Aggregate(0L, (acc, x) => acc * 10 + (x - '0'));
                numbers.Add(number);
            }

            var operand = lines[^1][lastSeparator + 1];
            var result = operand switch
            {
                '+' => numbers.Sum(),
                '*' => numbers.Aggregate(1L, (acc, x) => acc * x),
                _ => throw new InvalidOperationException($"Unknown operation character: {operand}")
            };
            total += result;
            lastSeparator = currentSeparator;
        }
        return total.ToString();
    }

    public static Solution FromText(string text)
    {
        return new Solution(text);
    }

}
