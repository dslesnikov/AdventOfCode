namespace AdventOfCode.Day4PrintingDepartment;

public enum Tile
{
    Nothing,
    RollOfPaper
}

public class Solution : ISolution, IFromLines<Solution, IReadOnlyList<Tile>>
{
    public static int Day => 4;

    private readonly IReadOnlyList<IReadOnlyList<Tile>> _tiles;

    private Solution(IReadOnlyList<IReadOnlyList<Tile>> tiles)
    {
        _tiles = tiles;
    }

    public string SolvePartOne()
    {
        var totalGoodRolls = 0;
        var field = _tiles.Select(x => x.ToArray()).ToArray();
        for (var row = 0; row < field.Length; row++)
        {
            for (var col = 0; col < field[row].Length; col++)
            {
                if (field[row][col] == Tile.RollOfPaper)
                {
                    var adjacentRolls = CountAdjacentRolls(row, col, field);
                    if (adjacentRolls < 4)
                    {
                        totalGoodRolls++;
                    }
                }
            }
        }
        return totalGoodRolls.ToString();
    }

    public string SolvePartTwo()
    {
        var field = _tiles.Select(x => x.ToArray()).ToArray();
        var tilesToClean = new List<(int Row, int Col)>();
        var totalRemoved = 0;
        do
        {
            foreach (var (row, col) in tilesToClean)
            {
                field[row][col] = Tile.Nothing;
                totalRemoved++;
            }
            tilesToClean.Clear();

            for (var row = 0; row < field.Length; row++)
            {
                for (var col = 0; col < field[row].Length; col++)
                {
                    if (field[row][col] == Tile.RollOfPaper)
                    {
                        var adjacentRolls = CountAdjacentRolls(row, col, field);
                        if (adjacentRolls < 4)
                        {
                            tilesToClean.Add((row, col));
                        }
                    }
                }
            }
        } while (tilesToClean.Count != 0);
        return totalRemoved.ToString();
    }

    private static int CountAdjacentRolls(int row, int col, Tile[][] field)
    {
        Span<(int Row, int Col)> directions =
        [
            (-1, -1),
            (-1, 0),
            (-1, 1),

            (0, -1),
            (0, 1),

            (1, -1),
            (1, 0),
            (1, 1)
        ];
        var count = 0;
        foreach (var (dRow, dCol) in directions)
        {
            var newRow = row + dRow;
            var newCol = col + dCol;
            if (newRow >= 0 && newRow < field.Length &&
                newCol >= 0 && newCol < field[newRow].Length &&
                field[newRow][newCol] == Tile.RollOfPaper)
            {
                count++;
            }
        }
        return count;
    }

    public static IReadOnlyList<Tile> ParseLine(string line)
    {
        return line.Select(x => x == '@' ? Tile.RollOfPaper : Tile.Nothing).ToList();
    }

    public static Solution FromParsed(IReadOnlyList<IReadOnlyList<Tile>> entries)
    {
        return new Solution(entries);
    }
}
