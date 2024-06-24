using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode._08_HauntedWasteland;

public class Solution(
    string sequence,
    IReadOnlyDictionary<string, (string Left, string Right)> paths) : ISolution<Solution>
{
    public static int Day => 8;

    public static Solution Parse(string s)
    {
        var split = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var sequence = split[0];
        var paths = split[1..]
            .Select(x =>
            {
                var from = x[..3];
                var left = x[7..10];
                var right = x[12..15];
                return (From: from, Left: left, Right: right);
            })
            .ToDictionary(x => x.From, x => (x.Left, x.Right));
        return new Solution(sequence, paths);
    }

    public string Part1()
    {
        const string start = "AAA";
        var current = start;
        var steps = 0;
        foreach (var instruction in sequence.Loop())
        {
            if (current == "ZZZ")
            {
                return steps.ToString();
            }
            var path = paths[current];
            current = instruction switch
            {
                'L' => path.Left,
                'R' => path.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
            steps++;
        }
        throw new UnreachableException();
    }

    public string Part2()
    {
        var current = paths.Keys.Where(x => x[^1] == 'A').ToArray();
        var targetLengths = new int[current.Length];
        for (var i = 0; i < current.Length; i++)
        {
            var steps = 0;
            var node = current[i];
            foreach (var instruction in sequence.Loop())
            {
                if (node[^1] == 'Z')
                {
                    targetLengths[i] = steps;
                    break;
                }
                node = instruction switch
                {
                    'L' => paths[node].Left,
                    'R' => paths[node].Right,
                    _ => throw new ArgumentOutOfRangeException()
                };
                steps++;
            }
        }
        var lcm = targetLengths
            .Select(x => new BigInteger(x))
            .Aggregate(new BigInteger(1), (acc, item) => acc * item / BigInteger.GreatestCommonDivisor(acc, item));
        return lcm.ToString();
    }
}