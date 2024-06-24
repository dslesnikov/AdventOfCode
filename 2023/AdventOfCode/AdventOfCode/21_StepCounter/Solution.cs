using System.Numerics;

namespace AdventOfCode._21_StepCounter;

public class Solution(IReadOnlyList<IReadOnlyList<char>> map) : ISolution<Solution>
{
    public static int Day => 21;

    public static Solution Parse(string s)
    {
        var map = s.Split('\n')
            .Select(line => line.ToCharArray())
            .ToList();
        return new Solution(map);
    }

    public string Part1()
    {
        var start = FindStart();
        var result = CountReachableTiles(start.Row, start.Col, 64);
        return result.ToString();
    }

    public string Part2()
    {
        var start = FindStart();
        var dataPoints = new[] { 65, 65 + 131, 65 + 131 * 2 }
            .Select(x => (X: x, Y: CountReachableTiles(start.Row, start.Col, x)))
            .ToArray();
        const long steps = 26501365;
        var result = new BigInteger(0);
        for (var i = 0; i < dataPoints.Length; i++)
        {
            var numerator = new BigInteger(1);
            var denominator = new BigInteger(1);
            for (var j = 0; j < dataPoints.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }
                numerator *= steps - dataPoints[j].X;
                denominator *= dataPoints[i].X - dataPoints[j].X;
            }
            result += dataPoints[i].Y * numerator / denominator;
        }
        return result.ToString();
    }

    private int CountReachableTiles(int startRow, int startCol, int steps)
    {
        var current = new HashSet<(int Row, int Col)>
        {
            (startRow, startCol)
        };
        var next = new HashSet<(int Row, int Col)>();
        Span<(int Row, int Col)> neighbors = stackalloc (int Row, int Col)[4];
        for (var i = 0; i < steps; i++)
        {
            foreach (var (row, col) in current)
            {
                var count = GetNeighbors(row, col, neighbors);
                for (var j = 0; j < count; j++)
                {
                    next.Add(neighbors[j]);
                }
            }
            (current, next) = (next, current);
            next.Clear();
        }
        return current.Count;
    }

    private int GetNeighbors(int row, int col, Span<(int Row, int Col)> neighbors)
    {
        var count = 0;
        if (map[Modulo(row - 1, map.Count)][Modulo(col, map[0].Count)] != '#')
        {
            neighbors[count++] = (row - 1, col);
        }
        if (map[Modulo(row + 1, map.Count)][Modulo(col, map[0].Count)] != '#')
        {
            neighbors[count++] = (row + 1, col);
        }
        if (map[Modulo(row, map.Count)][Modulo(col - 1, map[0].Count)] != '#')
        {
            neighbors[count++] = (row, col - 1);
        }
        if (map[Modulo(row, map.Count)][Modulo(col + 1, map[0].Count)] != '#')
        {
            neighbors[count++] = (row, col + 1);
        }
        return count;

        static int Modulo(int value, int max)
        {
            return (value % max + max) % max;
        }
    }

    private (int Row, int Col) FindStart()
    {
        for (var row = 0; row < map.Count; row++)
        {
            for (var col = 0; col < map[row].Count; col++)
            {
                if (map[row][col] == 'S')
                {
                    return (row, col);
                }
            }
        }
        return (-1, -1);
    }
}