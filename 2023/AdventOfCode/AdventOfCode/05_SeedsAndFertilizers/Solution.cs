using System.Text.RegularExpressions;

namespace AdventOfCode._05_SeedsAndFertilizers;

public partial class Solution(
    IReadOnlyList<uint> seeds,
    IReadOnlyList<Map> maps) : ISolution<Solution>
{
    [GeneratedRegex("seeds: ((?<seedId>\\d+) ?)+")]
    private static partial Regex SeedsPattern();
    [GeneratedRegex(@"map:\s+((?<mapEntry>\d+ \d+ \d+)\s+)+", RegexOptions.Singleline)]
    private static partial Regex MapPattern();

    public static int Day => 5;

    public static Solution Parse(string s)
    {
        var matches = MapPattern().Matches(s);
        var maps = new Map[matches.Count];
        for (var i = 0; i < maps.Length; i++)
        {
            var match = matches[i];
            var mapEntries = match.Groups["mapEntry"].Captures
                .Select(c =>
                {
                    var split = c.Value.Split(' ');
                    return new MapEntry(uint.Parse(split[1]), uint.Parse(split[0]), uint.Parse(split[2]));
                })
                .OrderBy(x => x.Source)
                .ToArray();
            maps[i] = new Map(mapEntries);
        }
        var seedsMatch = SeedsPattern().Match(s);
        var seeds = seedsMatch.Groups["seedId"].Captures
            .Select(c => uint.Parse(c.Value))
            .ToArray();
        return new Solution(seeds, maps);
    }

    public string Part1()
    {
        var min = TraverseMaps(seeds, maps);
        return min.ToString();
    }

    public string Part2()
    {
        var min = uint.MaxValue;
        Parallel.ForEach(seeds.Chunk(2), chunk =>
        {
            var seedRange = EnumerateSeedRange(chunk[0], chunk[1]);
            var localMin = TraverseMaps(seedRange, maps);
            Interlocked.Exchange(ref min, Math.Min(localMin, min));
        });
        return min.ToString();
    }

    private IEnumerable<uint> EnumerateSeedRange(uint start, uint length)
    {
        for (uint i = 0; i < length; i++)
        {
            yield return start + i;
        }
    }

    private static uint TraverseMaps(IEnumerable<uint> seeds, IReadOnlyList<Map> maps)
    {
        var min = uint.MaxValue;
        foreach (var seed in seeds)
        {
            var value = seed;
            foreach (var map in maps)
            {
                var newValue = value;
                foreach (var entry in map.Entries)
                {
                    if (entry.Source > value)
                    {
                        break;
                    }
                    if (entry.Source + entry.Length <= value)
                    {
                        continue;
                    }
                    newValue = value - entry.Source + entry.Destination;
                    break;
                }
                value = newValue;
            }
            min = Math.Min(value, min);
        }
        return min;
    }
}