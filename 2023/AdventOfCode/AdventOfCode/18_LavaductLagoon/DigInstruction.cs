using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode._18_LavaductLagoon;

public partial record DigInstruction(Direction Direction, int Distance, string Color) : ISimpleParsable<DigInstruction>
{
    [GeneratedRegex(@"(?<Direction>\w) (?<Distance>\d+) \(#(?<Color>\w+)\)")]
    private static partial Regex Pattern();

    public static DigInstruction Parse(string s)
    {
        var match = Pattern().Match(s);
        if (!match.Success)
        {
            throw new FormatException();
        }
        var directionValue = match.Groups["Direction"].Value;
        var direction = directionValue switch
        {
            "L" => Direction.Left,
            "R" => Direction.Right,
            "U" => Direction.Up,
            "D" => Direction.Down,
            _ => throw new FormatException()
        };
        var distance = int.Parse(match.Groups["Distance"].Value);
        var color = match.Groups["Color"].Value;
        return new DigInstruction(direction, distance, color);
    }

    public Instruction ExtractInstruction()
    {
        return new Instruction(Direction, Distance);
    }

    public Instruction DecodeInstruction()
    {
        var distance = int.Parse(Color.AsSpan(..^1), NumberStyles.HexNumber);
        var direction = Color[^1] switch
        {
            '0' => Direction.Right,
            '1' => Direction.Down,
            '2' => Direction.Left,
            '3' => Direction.Up,
            _ => throw new FormatException()
        };
        return new Instruction(direction, distance);
    }
}