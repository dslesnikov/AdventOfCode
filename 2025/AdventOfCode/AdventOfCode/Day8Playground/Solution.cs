namespace AdventOfCode.Day8Playground;

public class Solution : ISolution, IFromLines<Solution, Box>
{
    public static int Day => 8;

    private readonly IReadOnlyList<Box> _boxes;

    private Solution(IReadOnlyList<Box> boxes)
    {
        _boxes = boxes;
    }

    public string SolvePartOne()
    {
        var distances = new List<(int First, int Second, double Distance)>();
        for (var i = 0; i < _boxes.Count; i++)
        {
            for (var j = i + 1; j < _boxes.Count; j++)
            {
                distances.Add((i, j, _boxes[i].Distance(_boxes[j])));
            }
        }
        distances.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        var uf = new UnionFind(_boxes.Count);
        foreach (var (first, second, _) in distances.Take(1000))
        {
            uf.Union(first, second);
        }

        var sizes = new Dictionary<int, int>();
        for (var i = 0; i < _boxes.Count; i++)
        {
            var root = uf.Find(i);
            sizes.TryAdd(root, 0);
            sizes[root]++;
        }
        return sizes.Values.OrderByDescending(x => x).Take(3).Aggregate(1, (a, b) => a * b).ToString();
    }

    public string SolvePartTwo()
    {
        var distances = new List<(int First, int Second, double Distance)>();
        for (var i = 0; i < _boxes.Count; i++)
        {
            for (var j = i + 1; j < _boxes.Count; j++)
            {
                distances.Add((i, j, _boxes[i].Distance(_boxes[j])));
            }
        }
        distances.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        var uf = new UnionFind(_boxes.Count);
        var lastPair = (First: -1, Second: -1);
        foreach (var (first, second, _) in distances)
        {
            if (uf.ComponentsCount == 1)
            {
                break;
            }
            uf.Union(first, second);
            lastPair = (first, second);
        }
        
        return ((long)_boxes[lastPair.First].X * _boxes[lastPair.Second].X).ToString();
    }

    public static Box ParseLine(string line)
    {
        return Box.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<Box> entries)
    {
        return new Solution(entries);
    }
}