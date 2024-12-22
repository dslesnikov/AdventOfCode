namespace AdventOfCode.Day22MonkeyMarket;

public readonly record struct DiffSequence(int A, int B, int C, int D)
{
    public DiffSequence Next(int value)
    {
        return new DiffSequence(B, C, D, value);
    }
}

public class Solution : IFromLines<Solution, int>
{
    public static int Day => 22;

    private readonly IReadOnlyList<int> _numbers;

    private Solution(IReadOnlyList<int> numbers)
    {
        _numbers = numbers;
    }

    public string SolvePartOne()
    {
        var result = 0L;
        foreach (var number in _numbers)
        {
            var evolved = (long)number;
            for (var i = 0; i < 2000; i++)
            {
                evolved = Evolve(evolved);
            }

            result += evolved;
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var cumulative = new Dictionary<DiffSequence, Dictionary<int, int>>();
        for (var buyer = 0; buyer < _numbers.Count; buyer++)
        {
            var number = _numbers[buyer];
            var evolved = (long)number;
            var previousDigit = evolved % 10;
            evolved = Evolve(evolved);
            var newDigit = evolved % 10;
            var a = newDigit - previousDigit;

            previousDigit = newDigit;
            evolved = Evolve(evolved);
            newDigit = evolved % 10;
            var b = newDigit - previousDigit;

            previousDigit = newDigit;
            evolved = Evolve(evolved);
            newDigit = evolved % 10;
            var c = newDigit - previousDigit;

            previousDigit = newDigit;
            evolved = Evolve(evolved);
            newDigit = evolved % 10;
            var d = newDigit - previousDigit;

            var diffSequence = new DiffSequence((int)a, (int)b, (int)c, (int)d);
            if (cumulative.TryGetValue(diffSequence, out var buyerResults))
            {
                buyerResults.TryAdd(buyer, (int)newDigit);
            }
            else
            {
                cumulative[diffSequence] = new Dictionary<int, int> { { buyer, (int)newDigit } };
            }
            previousDigit = newDigit;
            for (var i = 4; i < 2000; i++)
            {
                evolved = Evolve(evolved);
                newDigit = evolved % 10;
                var diff = newDigit - previousDigit;
                diffSequence = diffSequence.Next((int)diff);
                if (cumulative.TryGetValue(diffSequence, out buyerResults))
                {
                    buyerResults.TryAdd(buyer, (int)newDigit);
                }
                else
                {
                    cumulative[diffSequence] = new Dictionary<int, int> { { buyer, (int)newDigit } };
                }
                previousDigit = newDigit;
            }
        }

        var result = int.MinValue;
        foreach (var (_, buyers) in cumulative)
        {
            var total = buyers.Sum(x => x.Value);
            if (total > result)
            {
                result = total;
            }
        }

        return result.ToString();
    }

    private long Evolve(long number)
    {
        number ^= number * 64;
        number %= 16777216;
        number ^= number / 32;
        number %= 16777216;
        number ^= number * 2048;
        number %= 16777216;
        return number;
    }

    public static int ParseLine(ReadOnlySpan<char> line)
    {
        return int.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<int> entries)
    {
        return new Solution(entries);
    }
}