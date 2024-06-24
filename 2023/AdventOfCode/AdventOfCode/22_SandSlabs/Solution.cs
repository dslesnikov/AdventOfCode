namespace AdventOfCode._22_SandSlabs;

public class Solution(BrickStack bricks) : ISolution<Solution>
{
    public static int Day => 22;

    public static Solution Parse(string s)
    {
        var bricks = s.Split('\n')
            .Select(BrickSlab.Parse)
            .ToList();
        var stack = SettleSlabs(bricks);
        return new Solution(stack);
    }

    public string Part1()
    {
        var onlySupporters = new HashSet<BrickSlab>();
        foreach (var (_, support) in bricks.Support)
        {
            if (support.Count == 1)
            {
                onlySupporters.Add(support[0]);
            }
        }
        var safeToDestroy = bricks.Bricks.Except(onlySupporters).Count();
        return safeToDestroy.ToString();
    }

    public string Part2()
    {
        var supportedBy = new Dictionary<BrickSlab, List<BrickSlab>>();
        var onlySupporters = new HashSet<BrickSlab>();
        foreach (var (brick, support) in bricks.Support)
        {
            if (support.Count == 1)
            {
                onlySupporters.Add(support[0]);
            }
            foreach (var slab in support)
            {
                if (!supportedBy.TryGetValue(slab, out var list))
                {
                    supportedBy[slab] = list = new List<BrickSlab>();
                }
                list.Add(brick);
            }
        }
        var sum = 0;
        foreach (var brick in onlySupporters)
        {
            var current = new List<BrickSlab> { brick };
            var next = new List<BrickSlab>();
            var moved = new HashSet<BrickSlab> { brick };
            while (current.Count > 0)
            {
                foreach (var slab in current)
                {
                    if (!supportedBy.TryGetValue(slab, out var list))
                    {
                        continue;
                    }
                    foreach (var supportedSlab in list)
                    {
                        var remainingSupport = bricks.Support[supportedSlab]
                            .Count(x => !moved.Contains(x));
                        if (remainingSupport == 0)
                        {
                            moved.Add(supportedSlab);
                            next.Add(supportedSlab);
                        }
                    }
                }
                (current, next) = (next, current);
                next.Clear();
            }
            sum += moved.Count - 1;
        }
        return sum.ToString();
    }

    private static BrickStack SettleSlabs(IReadOnlyList<BrickSlab> bricks)
    {
        var byHeight = new Dictionary<int, List<BrickSlab>>();
        foreach (var brick in bricks)
        {
            var min = Math.Min(brick.To.Z, brick.From.Z);
            var max = Math.Max(brick.To.Z, brick.From.Z);
            for (var height = min; height <= max; height++)
            {
                if (!byHeight.TryGetValue(height, out var list))
                {
                    byHeight[height] = list = new List<BrickSlab>();
                }
                list.Add(brick);
            }
        }
        var settledBricks = new List<BrickSlab>(bricks.Count);
        var settledByHeight = new Dictionary<int, List<BrickSlab>>();
        var support = new Dictionary<BrickSlab, IReadOnlyList<BrickSlab>>();
        var processed = new HashSet<BrickSlab>();
        var heights = byHeight.Keys.Order().ToArray();
        foreach (var height in heights)
        {
            if (!byHeight.TryGetValue(height, out var levelBricks))
            {
                continue;
            }
            foreach (var brick in levelBricks)
            {
                if (!processed.Add(brick))
                {
                    continue;
                }
                var supportingBricks = FindSupport(brick, settledByHeight);
                var z = supportingBricks.Count == 0
                    ? 1
                    : Math.Max(supportingBricks[0].From.Z, supportingBricks[0].To.Z) + 1;
                var diff = height - z;
                var settledBrick = new BrickSlab(
                    brick.From with { Z = brick.From.Z - diff },
                    brick.To with { Z = brick.To.Z - diff });
                support[settledBrick] = supportingBricks;
                settledBricks.Add(settledBrick);
                var minZ = Math.Min(settledBrick.From.Z, settledBrick.To.Z);
                var maxZ = Math.Max(settledBrick.From.Z, settledBrick.To.Z);
                for (var i = minZ; i <= maxZ; i++)
                {
                    if (!settledByHeight.TryGetValue(i, out var list))
                    {
                        settledByHeight[i] = list = new List<BrickSlab>();
                    }
                    list.Add(settledBrick);
                }
            }
        }
        return new BrickStack(
            settledBricks,
            settledByHeight.ToDictionary(x => x.Key, x => (IReadOnlyList<BrickSlab>)x.Value),
            support);
    }

    private static IReadOnlyList<BrickSlab> FindSupport(
        BrickSlab slab,
        IReadOnlyDictionary<int, List<BrickSlab>> byHeight)
    {
        var minZ = Math.Min(slab.To.Z, slab.From.Z);
        for (var z = minZ - 1; z > 0; z--)
        {
            if (!byHeight.TryGetValue(z, out var lowerBricks))
            {
                continue;
            }
            var intersectingBricks = lowerBricks
                .Where(br => br.From.X <= slab.To.X &&
                             br.To.X >= slab.From.X &&
                             br.From.Y <= slab.To.Y &&
                             br.To.Y >= slab.From.Y)
                .ToArray();
            if (intersectingBricks.Length > 0)
            {
                return intersectingBricks;
            }
        }
        return Array.Empty<BrickSlab>();
    }
}