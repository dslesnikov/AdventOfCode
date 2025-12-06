namespace AdventOfCode.Day1SecretEntrance;

public record Rotation(Direction Direction, int Value)
{
    public int Apply(int current)
    {
        var value = Direction switch
        {
            Direction.Left => current - Value % 100,
            Direction.Right => current + Value % 100,
            _ => throw new ArgumentOutOfRangeException()
        };
        value = value < 0 ? value + 100 : value;
        return value % 100;
    }
    public (int Value, int Zeros) ApplyCountingZeros(int current)
    {
        var value = Direction switch
        {
            Direction.Left => current - Value % 100,
            Direction.Right => current + Value % 100,
            _ => throw new ArgumentOutOfRangeException()
        };
        var zeros = Value / 100;
        if (value <= 0)
        {
            if (current != 0)
            {
                zeros++;
            }
            value += 100;
        }
        else if (value >= 100)
        {
            zeros++;
        }

        return (value % 100, zeros);
    }

    public static Rotation Parse(ReadOnlySpan<char> s)
    {
        var direction = s[0] == 'L' ? Direction.Left : Direction.Right;
        var value = int.Parse(s[1..]);
        return new Rotation(direction, value);
    }
}