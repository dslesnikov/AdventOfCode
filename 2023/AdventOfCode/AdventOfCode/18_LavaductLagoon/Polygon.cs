namespace AdventOfCode._18_LavaductLagoon;

public record Polygon(IReadOnlyList<Point> Points)
{
    public static Polygon FromInstructions(IEnumerable<Instruction> instructions)
    {
        var start = new Point(0, 0);
        var points = new List<Point> { start };
        var current = start;
        foreach (var instruction in instructions)
        {
            current = current.Move(instruction);
            points.Add(current);
        }
        return new Polygon(points);
    }

    public long GetArea()
    {
        var area = 0L;
        var perimeter = 0L;
        for (var i = 0; i < Points.Count - 1; i++)
        {
            var a = Points[i];
            var b = Points[i + 1];
            checked
            {
                area += a.X * (long)b.Y - a.Y * (long)b.X;
                perimeter += Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            }
        }
        var result = checked(Math.Abs(area + perimeter) / 2 + 1);
        return result;
    }
}