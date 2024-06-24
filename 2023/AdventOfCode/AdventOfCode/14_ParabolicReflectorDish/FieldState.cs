namespace AdventOfCode._14_ParabolicReflectorDish;

public sealed class FieldState(Cell[] balls) : IEquatable<FieldState>
{
    private readonly Cell[] _balls = balls;

    public int GetNorthBeamLoad(int height)
    {
        return _balls.Sum(t => height - t.Row);
    }

    public bool Equals(FieldState? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (_balls.Length != other._balls.Length)
        {
            return false;
        }
        for (var i = 0; i < _balls.Length; i++)
        {
            if (_balls[i] != other._balls[i])
            {
                return false;
            }
        }
        return true;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as FieldState);
    }

    public override int GetHashCode()
    {
        return _balls.Aggregate(0, HashCode.Combine);
    }
}