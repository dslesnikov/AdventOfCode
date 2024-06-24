namespace AdventOfCode._02_CubeConundrum;

public record CubeSet(int Red, int Green, int Blue) : IParsable<CubeSet>
{
    public static CubeSet Parse(string input, IFormatProvider? provider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        if (TryParse(input, provider, out var result))
        {
            return result;
        }
        throw new ArgumentException("Failed to parse CubeSet", nameof(input));
    }

    public static bool TryParse(string? input, IFormatProvider? provider, out CubeSet result)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            result = default!;
            return false;
        }
        var colors = input.Split(", ");
        var (red, green, blue) = (0, 0, 0);
        foreach (var color in colors)
        {
            var split = color.Split(' ');
            switch (split[1])
            {
                case "red":
                    red = int.Parse(split[0]);
                    break;
                case "green":
                    green = int.Parse(split[0]);
                    break;
                case "blue":
                    blue = int.Parse(split[0]);
                    break;
            }
        }
        result = new CubeSet(red, green, blue);
        return true;
    }
}