namespace AdventOfCode.Day04CeresSearch;

public class Solution : IFromLines<Solution, string>
{
    public static int Day => 4;

    private readonly IReadOnlyList<string> _lines;

    private Solution(IReadOnlyList<string> lines)
    {
        _lines = lines;
    }

    public string SolvePartOne()
    {
        var directHorizontal = _lines.Sum(line => Count(line, "XMAS"));
        var reverseHorizontal = _lines.Sum(line => Count(line, "SAMX"));
        var directVertical = Enumerable.Range(0, _lines[0].Length)
            .Select(index => _lines.Select(line => line[index]))
            .Sum(seq => Count(seq, "XMAS"));
        var reverseVertical = Enumerable.Range(0, _lines[0].Length)
            .Select(index => _lines.Select(line => line[index]))
            .Sum(seq => Count(seq, "SAMX"));
        var diagonalUpLeft = 0;
        var diagonalUpRight = 0;
        var diagonalDownLeft = 0;
        var diagonalDownRight = 0;
        for (var row = 0; row < _lines.Count; row++)
        {
            for (var col = 0; col < _lines[row].Length; col++)
            {
                if (row >= 3 && col >= 3)
                {
                    if (_lines[row][col] == 'X'
                        && _lines[row - 1][col - 1] == 'M'
                        && _lines[row - 2][col - 2] == 'A'
                        && _lines[row - 3][col - 3] == 'S')
                    {
                        diagonalUpLeft++;
                    }
                }
                if (row >= 3 && col < _lines[row].Length - 3)
                {
                    if (_lines[row][col] == 'X'
                        && _lines[row - 1][col + 1] == 'M'
                        && _lines[row - 2][col + 2] == 'A'
                        && _lines[row - 3][col + 3] == 'S')
                    {
                        diagonalUpRight++;
                    }
                }
                if (row < _lines.Count - 3 && col >= 3)
                {
                    if (_lines[row][col] == 'X'
                        && _lines[row + 1][col - 1] == 'M'
                        && _lines[row + 2][col - 2] == 'A'
                        && _lines[row + 3][col - 3] == 'S')
                    {
                        diagonalDownLeft++;
                    }
                }

                if (row < _lines.Count - 3 && col < _lines[row].Length - 3)
                {
                    if (_lines[row][col] == 'X'
                        && _lines[row + 1][col + 1] == 'M'
                        && _lines[row + 2][col + 2] == 'A'
                        && _lines[row + 3][col + 3] == 'S')
                    {
                        diagonalDownRight++;
                    }
                }
            }
        }
        var diagonal = diagonalUpLeft + diagonalUpRight + diagonalDownLeft + diagonalDownRight;
        return (directHorizontal + reverseHorizontal + directVertical + reverseVertical + diagonal).ToString();
    }

    public string SolvePartTwo()
    {
        var count = 0;
        Span<char> corners = stackalloc char[4];
        for (var row = 1; row < _lines.Count - 1; row++)
        {
            for (var col = 1; col < _lines[row].Length - 1; col++)
            {
                if (_lines[row][col] != 'A')
                {
                    continue;
                }
                corners[0] = _lines[row - 1][col - 1];
                corners[1] = _lines[row - 1][col + 1];
                corners[2] = _lines[row + 1][col - 1];
                corners[3] = _lines[row + 1][col + 1];
                var mCount = corners.Count('M');
                var sCount = corners.Count('S');
                if (mCount == 2 && sCount == 2 && corners[0] != corners[3])
                {
                    count++;
                }
            }
        }
        return count.ToString();
    }

    private int Count(IEnumerable<char> text, ReadOnlySpan<char> pattern)
    {
        var count = 0;
        var state = 0;
        foreach (var c in text)
        {
            if (c == pattern[state])
            {
                state++;
                if (state == pattern.Length)
                {
                    state = 0;
                    count++;
                }
            }
            else
            {
                state = c == pattern[0] ? 1 : 0;
            }
        }
        return count;
    }

    public static string ParseLine(ReadOnlySpan<char> line)
    {
        return new string(line);
    }

    public static Solution FromParsed(IReadOnlyList<string> entries)
    {
        return new Solution(entries);
    }
}