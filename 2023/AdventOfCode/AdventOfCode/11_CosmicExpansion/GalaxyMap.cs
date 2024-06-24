using System.Collections.Immutable;

namespace AdventOfCode._11_CosmicExpansion;

public record GalaxyMap(
    ImmutableArray<Galaxy> Galaxies,
    ImmutableArray<int> EmptyColumns,
    ImmutableArray<int> EmptyRows);