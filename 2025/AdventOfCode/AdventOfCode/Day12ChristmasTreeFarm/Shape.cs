using System.Collections.Immutable;

namespace AdventOfCode.Day12ChristmasTreeFarm;

public record Shape(ImmutableArray<ImmutableArray<bool>> Mask)
{
    public static Shape Parse(string input)
    {
        var mask = input.Split('\n')
            .Skip(1)
            .Select(line => line.Select(x => x == '#').ToImmutableArray())
            .ToImmutableArray();
        return new Shape(mask);
    }
}