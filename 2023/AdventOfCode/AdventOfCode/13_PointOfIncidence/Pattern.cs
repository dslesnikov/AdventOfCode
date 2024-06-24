using System.Numerics;

namespace AdventOfCode._13_PointOfIncidence;

public record Pattern(int[] Rows, int[] Columns) : ISimpleParsable<Pattern>
{
    public static Pattern Parse(string s)
    {
        var lines = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var rows = new int[lines.Length];
        var columns = new int[lines[0].Length];
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (var j = 0; j < line.Length; j++)
            {
                var c = line[j];
                if (c == '#')
                {
                    rows[i] |= 1 << j;
                    columns[j] |= 1 << i;
                }
            }
        }
        return new Pattern(rows, columns);
    }

    public ReflectionPoint FindPerfectReflectionPoint()
    {
        return FindReflectionPoint(0);
    }

    public ReflectionPoint FindSingleSmudgeReflectionPoint()
    {
        return FindReflectionPoint(1);
    }

    private ReflectionPoint FindReflectionPoint(int diff)
    {
        var verticalPoint = GetReflectionPoint(Columns, diff);
        if (verticalPoint >= 0)
        {
            return new ReflectionPoint(verticalPoint, ReflectionType.Vertical);
        }
        var horizontalPoint = GetReflectionPoint(Rows, diff);
        return new ReflectionPoint(horizontalPoint, ReflectionType.Horizontal);
    }

    private static int GetReflectionPoint(int[] array, int targetDiff)
    {
        foreach (var (index, diff) in EnumerateReflectionDiffs(array))
        {
            if (diff == targetDiff)
            {
                return index;
            }
        }
        return -1;
    }

    private static IEnumerable<(int Index, int Diff)> EnumerateReflectionDiffs(int[] array)
    {
        for (var i = 0; i < array.Length - 1; i++)
        {
            var left = i;
            var right = i + 1;
            var totalDiff = 0;
            while (left >= 0 && right < array.Length)
            {
                var diff = array[left] ^ array[right];
                totalDiff += BitOperations.PopCount((uint)diff);
                left--;
                right++;
            }
            yield return (i, totalDiff);
        }
    }
}