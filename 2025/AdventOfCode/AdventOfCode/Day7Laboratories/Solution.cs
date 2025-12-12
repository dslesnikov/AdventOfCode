namespace AdventOfCode.Day7Laboratories;

public enum Tile
{
    Empty,
    Start,
    Splitter,
    Beam
}

public class Solution : ISolution, IFromLines<Solution, IReadOnlyList<Tile>>
{
    public static int Day => 7;

    private readonly IReadOnlyList<IReadOnlyList<Tile>> _field;

    private Solution(IReadOnlyList<IReadOnlyList<Tile>> field)
    {
        _field = field;
    }

    public string SolvePartOne()
    {
        var field = _field.Select(x => x.ToArray()).ToArray();
        var totalSplits = 0;
        for (var row = 1; row < field.Length; row++)
        {
            for (var col = 0; col < field[row].Length; col++)
            {
                if (field[row - 1][col] is Tile.Beam or Tile.Start)
                {
                    switch (field[row][col])
                    {
                        case Tile.Empty:
                            field[row][col] = Tile.Beam;
                            break;
                        case Tile.Splitter:
                            totalSplits++;
                            if (col > 1)
                            {
                                field[row][col - 1] = Tile.Beam;
                            }

                            if (col < field[row].Length - 1)
                            {
                                field[row][col + 1] = Tile.Beam;
                            }
                            break;
                        case Tile.Beam:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        return totalSplits.ToString();
    }

    public string SolvePartTwo()
    {
        var field = _field.Select(x => x.ToArray()).ToArray();
        var count = _field.Select(row => row.Select(_ => 0L).ToArray()).ToArray();
        for (var col = 0; col < field[0].Length; col++)
        {
            if (field[0][col] == Tile.Start)
            {
                count[0][col] = 1;
            }
        }
        for (var row = 1; row < field.Length; row++)
        {
            for (var col = 0; col < field[row].Length; col++)
            {
                if (field[row - 1][col] is Tile.Beam or Tile.Start)
                {
                    switch (field[row][col])
                    {
                        case Tile.Empty:
                            field[row][col] = Tile.Beam;
                            count[row][col] += count[row - 1][col];
                            break;
                        case Tile.Splitter:
                            if (col > 0)
                            {
                                field[row][col - 1] = Tile.Beam;
                                count[row][col - 1] += count[row - 1][col];
                            }

                            if (col < field[row].Length - 1)
                            {
                                field[row][col + 1] = Tile.Beam;
                                count[row][col + 1] += count[row - 1][col];
                            }
                            break;
                        case Tile.Beam:
                            count[row][col] += count[row - 1][col];
                            break;
                    }
                }
            }
        }
        return count[^1].Sum().ToString();
    }

    public static IReadOnlyList<Tile> ParseLine(string line)
    {
        return line
            .Select(c => c switch
            {
                '.' => Tile.Empty,
                '^' => Tile.Splitter,
                _ => Tile.Start
            })
            .ToArray();
    }

    public static Solution FromParsed(IReadOnlyList<IReadOnlyList<Tile>> entries)
    {
        return new Solution(entries);
    }
}