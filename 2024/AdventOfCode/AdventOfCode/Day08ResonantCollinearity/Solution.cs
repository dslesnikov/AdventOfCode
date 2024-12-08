namespace AdventOfCode.Day08ResonantCollinearity;

public class Solution : IFromLines<Solution, string>
{
    public static int Day => 8;

    private readonly IReadOnlyList<string> _lines;
    private readonly IReadOnlyDictionary<char, IReadOnlyList<(int Row, int Col)>> _antennas;

    private Solution(IReadOnlyList<string> lines)
    {
        _lines = lines;
        var antennas = new Dictionary<char, IReadOnlyList<(int Row, int Col)>>();
        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            for (var col = 0; col < line.Length; col++)
            {
                var c = line[col];
                if (c == '.')
                {
                    continue;
                }
                if (!antennas.TryGetValue(c, out var positions))
                {
                    positions = new List<(int Row, int Col)>();
                    antennas[c] = positions;
                }
                ((List<(int Row, int Col)>)positions).Add((row, col));
            }
        }
        _antennas = antennas;
    }

    public string SolvePartOne()
    {
        var antiNodes = new HashSet<(int Row, int Col)>();
        foreach (var compatibleAntennas in _antennas)
        {
            var list = compatibleAntennas.Value;
            for (var i = 0; i < list.Count; i++)
            {
                var first = list[i];
                for (var j = i + 1; j < list.Count; j++)
                {
                    var second = list[j];
                    var xDiff = second.Col - first.Col;
                    var x1 = first.Col - xDiff;
                    var x2 = second.Col + xDiff;
                    var yDiff = second.Row - first.Row;
                    var y1 = first.Row - yDiff;
                    var y2 = second.Row + yDiff;
                    if (x1 >= 0 && x1 < _lines[0].Length &&
                        y1 >= 0 && y1 < _lines.Count)
                    {
                        antiNodes.Add((y1, x1));
                    }
                    if (x2 >= 0 && x2 < _lines[0].Length &&
                        y2 >= 0 && y2 < _lines.Count)
                    {
                        antiNodes.Add((y2, x2));
                    }
                }
            }
        }

        return antiNodes.Count.ToString();
    }

    public string SolvePartTwo()
    {
        var antiNodes = new HashSet<(int Row, int Col)>();
        foreach (var compatibleAntennas in _antennas)
        {
            var list = compatibleAntennas.Value;
            for (var i = 0; i < list.Count; i++)
            {
                var first = list[i];
                for (var j = i + 1; j < list.Count; j++)
                {
                    var second = list[j];
                    var xDiff = second.Col - first.Col;
                    var x1 = first.Col - xDiff;
                    var x2 = second.Col + xDiff;
                    var yDiff = second.Row - first.Row;
                    var y1 = first.Row - yDiff;
                    var y2 = second.Row + yDiff;
                    while (x1 >= 0 && x1 < _lines[0].Length &&
                           y1 >= 0 && y1 < _lines.Count)
                    {
                        antiNodes.Add((y1, x1));
                        x1 -= xDiff;
                        y1 -= yDiff;
                    }
                    while (x2 >= 0 && x2 < _lines[0].Length &&
                           y2 >= 0 && y2 < _lines.Count)
                    {
                        antiNodes.Add((y2, x2));
                        x2 += xDiff;
                        y2 += yDiff;
                    }
                    antiNodes.Add(second);
                }
                antiNodes.Add(first);
            }
        }

        return antiNodes.Count.ToString();
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