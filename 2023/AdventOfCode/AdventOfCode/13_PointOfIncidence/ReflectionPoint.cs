namespace AdventOfCode._13_PointOfIncidence;

public record ReflectionPoint(int Index, ReflectionType Type)
{
    public int Summarize()
    {
        return Type switch
        {
            ReflectionType.Vertical => Index + 1,
            ReflectionType.Horizontal => (Index + 1) * 100,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}