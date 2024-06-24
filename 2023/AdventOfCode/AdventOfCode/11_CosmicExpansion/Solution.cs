using System.Collections.Immutable;

namespace AdventOfCode._11_CosmicExpansion;

public class Solution(GalaxyMap galaxyMap) : ISolution<Solution>
{
    public static int Day => 11;

    public static Solution Parse(string s)
    {
        var lines = s.Split('\n');
        var galaxies = new List<Galaxy>();
        var emptyRowsSet = Enumerable.Range(0, lines.Length).ToHashSet();
        var emptyColumnsSet = Enumerable.Range(0, lines[0].Length).ToHashSet();
        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            for (var col = 0; col < line.Length; col++)
            {
                var c = line[col];
                if (c != '#')
                {
                    continue;
                }
                emptyRowsSet.Remove(row);
                emptyColumnsSet.Remove(col);
                galaxies.Add(new Galaxy(row, col));
            }
        }
        var emptyRows = new int[lines.Length];
        emptyRows[0] = emptyRowsSet.Contains(0) ? 1 : 0;
        for (var i = 1; i < emptyRows.Length; i++)
        {
            emptyRows[i] = emptyRowsSet.Contains(i)
                ? emptyRows[i - 1] + 1
                : emptyRows[i - 1];
        }
        var emptyColumns = new int[lines[0].Length];
        emptyColumns[0] = emptyColumnsSet.Contains(0) ? 1 : 0;
        for (var i = 1; i < emptyColumns.Length; i++)
        {
            emptyColumns[i] = emptyColumnsSet.Contains(i)
                ? emptyColumns[i - 1] + 1
                : emptyColumns[i - 1];
        }
        var galaxyMap = new GalaxyMap(
            galaxies.ToImmutableArray(),
            emptyColumns.ToImmutableArray(),
            emptyRows.ToImmutableArray());
        return new Solution(galaxyMap);
    }

    public string Part1()
    {
        var result = GetDistanceSum(2);
        return result.ToString();
    }

    public string Part2()
    {
        var result = GetDistanceSum(1000000);
        return result.ToString();
    }

    private long GetDistanceSum(int emptyMultiplier)
    {
        var result = 0L;
        var galaxies = galaxyMap.Galaxies;
        for (var i = 0; i < galaxies.Length; i++)
        {
            for (var j = i + 1; j < galaxies.Length; j++)
            {
                var from = galaxies[i];
                var to = galaxies[j];
                var rawRowsDistance = Math.Abs(to.Row - from.Row);
                var emptyRowsDistance = (galaxyMap.EmptyRows[Math.Max(to.Row, from.Row)] - galaxyMap.EmptyRows[Math.Min(to.Row, from.Row)])
                                        * (emptyMultiplier - 1);
                var rowDiff = rawRowsDistance + emptyRowsDistance;
                var rawColDistance = Math.Abs(to.Col - from.Col);
                var emptyColDistance = (galaxyMap.EmptyColumns[Math.Max(to.Col, from.Col)] - galaxyMap.EmptyColumns[Math.Min(to.Col, from.Col)])
                                        * (emptyMultiplier - 1);
                var colDiff = rawColDistance + emptyColDistance;
                checked
                {
                    var distance = rowDiff + colDiff;
                    result += distance;
                }
            }
        }
        return result;
    }
}