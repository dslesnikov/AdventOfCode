namespace AdventOfCode._09_MirageMaintenance;

public class Solution(IReadOnlyList<IReadOnlyList<int>> histories) : ISolution<Solution>
{
    public static int Day => 9;

    public static Solution Parse(string s)
    {
        var lines = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var histories = lines
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray())
            .ToArray();
        return new Solution(histories);
    }

    public string Part1()
    {
        var results = ExtrapolateHistories(true);
        return results.Sum().ToString();
    }

    public string Part2()
    {
        var results = ExtrapolateHistories(false);
        return results.Sum().ToString();
    }

    private IEnumerable<int> ExtrapolateHistories(bool direction)
    {
        return histories.Select(h => ExtrapolateHistoryForward(direction ? h : h.Reverse().ToArray()));
    }

    private static int ExtrapolateHistoryForward(IReadOnlyList<int> history)
    {
        var extrapolated = new int[history.Count][];
        for (var i = 0; i < history.Count; i++)
        {
            extrapolated[i] = new int[history.Count + 1];
            extrapolated[0][i] = history[i];
        }
        int rowIndex;
        for (rowIndex = 1; rowIndex < history.Count; rowIndex++)
        {
            var zeros = 0;
            for (var j = 0; j < history.Count - rowIndex; j++)
            {
                extrapolated[rowIndex][j] = extrapolated[rowIndex - 1][j + 1] - extrapolated[rowIndex - 1][j];
                if (extrapolated[rowIndex][j] == 0)
                {
                    zeros++;
                }
            }
            if (zeros == history.Count - rowIndex)
            {
                break;
            }
        }
        rowIndex--;
        for (; rowIndex >= 0; rowIndex--)
        {
            extrapolated[rowIndex][history.Count - rowIndex] =
                extrapolated[rowIndex + 1][history.Count - rowIndex - 1]
                + extrapolated[rowIndex][history.Count - rowIndex - 1];
        }
        return extrapolated[0][^1];
    }
}