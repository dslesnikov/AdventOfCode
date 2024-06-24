namespace AdventOfCode._18_LavaductLagoon;

public record Point(int X, int Y)
{
    public Point Move(Instruction instruction)
    {
        return instruction.Direction switch
        {
            Direction.Up => this with { Y = Y - instruction.Distance },
            Direction.Down => this with { Y = Y + instruction.Distance },
            Direction.Left => this with { X = X - instruction.Distance },
            Direction.Right => this with { X = X + instruction.Distance },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}