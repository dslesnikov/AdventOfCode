namespace AdventOfCode.Day12ChristmasTreeFarm;

public class Solution : ISolution, IFromText<Solution>
{
    public static int Day => 12;

    private readonly IReadOnlyList<Shape> _shapes;
    private readonly IReadOnlyList<Region> _regions;

    private Solution(IReadOnlyList<Shape> shapes, IReadOnlyList<Region> regions)
    {
        _shapes = shapes;
        _regions = regions;
    }

    public string SolvePartOne()
    {
        return _regions.Count(region =>
        {
            var shapeSlots = region.Width / 3 * (region.Length / 3);
            var shapesRequired = region.ShapeCount.Sum();
            return shapesRequired <= shapeSlots;
        }).ToString();
    }

    public string SolvePartTwo()
    {
        return string.Empty;
    }
    
    public static Solution FromText(string text)
    {
        var segments = text.Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var shapesRaw = segments[..6];
        var shapes = shapesRaw.Select(Shape.Parse).ToList();
        var regions = segments[6]
            .Split('\n')
            .Select(Region.Parse).ToList();
        return new Solution(shapes, regions);
    }
}