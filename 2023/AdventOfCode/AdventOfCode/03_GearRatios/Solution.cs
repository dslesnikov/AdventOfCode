namespace AdventOfCode._03_GearRatios;

public class Solution(
    IReadOnlyList<string> lines,
    IReadOnlyList<PartNumber> partNumbers) : ISolution<Solution>
{
    public static int Day => 3;

    public static Solution Parse(string s)
    {
        var lines = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var partNumbers = ParsePartNumbers(lines);
        return new Solution(lines, partNumbers);
    }

    public string Part1()
    {
        return partNumbers.Sum(x => x.Value).ToString();
    }

    public string Part2()
    {
        var gearRatios = ParseGearRatios();
        return gearRatios.Sum().ToString();
    }

    private IReadOnlyList<int> ParseGearRatios()
    {
        var gearRatios = new List<int>();
        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] != '*')
                {
                    continue;
                }
                var numbers = GetAdjacentNumbers(row, i);
                if (numbers.Count == 2)
                {
                    gearRatios.Add(numbers[0].Value * numbers[1].Value);
                }
            }
        }
        return gearRatios;
    }

    private IReadOnlyList<PartNumber> GetAdjacentNumbers(int row, int index)
    {
        return partNumbers
            .Where(x => Math.Abs(row - x.Row) <= 1 && x.Index <= index + 1 && x.Index + x.Length >= index)
            .ToArray();
    }

    private static IReadOnlyList<PartNumber> ParsePartNumbers(IReadOnlyList<string> lines)
    {
        var partNumbers = new List<PartNumber>();
        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            var index = 0;
            while (index < line.Length)
            {
                if (!char.IsDigit(line[index]))
                {
                    index++;
                    continue;
                }

                var length = 0;
                while (index + length < line.Length && char.IsDigit(line[index + length]))
                {
                    length++;
                }

                if (IsAdjacentToSymbol(lines, row, index, length))
                {
                    var partNumber = int.Parse(line.AsSpan(index, length));
                    partNumbers.Add(new PartNumber(partNumber, row, index, length));
                }
                index += length;
            }
        }

        return partNumbers;
    }

    private static bool IsAdjacentToSymbol(IReadOnlyList<string> rows, int row, int start, int length)
    {
        if (start > 0 && rows[row][start - 1] != '.')
        {
            return true;
        }
        if (start + length < rows[row].Length && rows[row][start + length] != '.')
        {
            return true;
        }
        if (row > 0)
        {
            for (var i = start - 1; i < start + length + 1; i++)
            {
                if (i >= 0 && i < rows[row - 1].Length && rows[row - 1][i] != '.')
                {
                    return true;
                }
            }
        }
        if (row < rows.Count - 1)
        {
            for (var i = start - 1; i < start + length + 1; i++)
            {
                if (i >= 0 && i < rows[row + 1].Length && rows[row + 1][i] != '.')
                {
                    return true;
                }
            }
        }
        return false;
    }
}