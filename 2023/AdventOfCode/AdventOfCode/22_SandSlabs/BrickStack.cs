namespace AdventOfCode._22_SandSlabs;

public class BrickStack(
    IReadOnlyList<BrickSlab> bricks,
    IReadOnlyDictionary<int, IReadOnlyList<BrickSlab>> byHeight,
    IReadOnlyDictionary<BrickSlab, IReadOnlyList<BrickSlab>> support)
{
    public IReadOnlyList<BrickSlab> Bricks { get; } = bricks;
    public IReadOnlyDictionary<int, IReadOnlyList<BrickSlab>> ByHeight { get; } = byHeight;
    public IReadOnlyDictionary<BrickSlab, IReadOnlyList<BrickSlab>> Support { get; } = support;
}