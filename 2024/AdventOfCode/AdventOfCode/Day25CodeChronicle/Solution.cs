namespace AdventOfCode.Day25CodeChronicle;

public record Lock(int A, int B, int C, int D, int E)
{
    public bool CanFit(Key key)
    {
        return A + key.A < 6 &&
               B + key.B < 6 &&
               C + key.C < 6 &&
               D + key.D < 6 &&
               E + key.E < 6;
    }

    public static Lock FromMap(string text)
    {
        Span<int> heights = stackalloc int[5];
        var lines = text.Split('\n');
        for (var col = 0; col < lines[0].Length; col++)
        {
            int row;
            for (row = 0; row < lines.Length; row++)
            {
                if (lines[row][col] == '.')
                {
                    break;
                }
            }
            heights[col] = row - 1;
        }
        return new Lock(heights[0], heights[1], heights[2], heights[3], heights[4]);
    }
}

public record Key(int A, int B, int C, int D, int E)
{
    public static Key FromMap(string text)
    {
        Span<int> heights = stackalloc int[5];
        var lines = text.Split('\n');
        for (var col = 0; col < lines[0].Length; col++)
        {
            int row;
            for (row = 0; row < lines.Length; row++)
            {
                if (lines[row][col] == '#')
                {
                    break;
                }
            }
            heights[col] = lines.Length - row - 1;
        }
        return new Key(heights[0], heights[1], heights[2], heights[3], heights[4]);
    }
}

public class Solution : IFromText<Solution>
{
    public static int Day => 25;

    private readonly IReadOnlyList<Key> _keys;
    private readonly IReadOnlyList<Lock> _locks;

    private Solution(IReadOnlyList<Key> keys, IReadOnlyList<Lock> locks)
    {
        _keys = keys;
        _locks = locks;
    }

    public string SolvePartOne()
    {
        var count = 0;
        foreach (var key in _keys)
        {
            foreach (var @lock in _locks)
            {
                if (@lock.CanFit(key))
                {
                    count++;
                }
            }
        }
        return count.ToString();
    }

    public string SolvePartTwo()
    {
        return "Merry Christmas!";
    }

    public static Solution FromText(string text)
    {
        var split = text.Split("\n\n");
        var keys = new List<Key>();
        var locks = new List<Lock>();
        foreach (var map in split)
        {
            if (map[0] == '#')
            {
                locks.Add(Lock.FromMap(map));
            }
            else
            {
                keys.Add(Key.FromMap(map));
            }
        }
        return new Solution(keys, locks);
    }
}