using System.Text.RegularExpressions;

namespace AdventOfCode._19_Aplenty;

public partial record Part(int CoolLooking, int Musical, int Aerodynamic, int Shiny) : ISimpleParsable<Part>
{
    [GeneratedRegex(@"{x=(?<CoolLooking>\d+),m=(?<Musical>\d+),a=(?<Aerodynamic>\d+),s=(?<Shiny>\d+)}")]
    private static partial Regex Pattern();

    public static Part Parse(string s)
    {
        var match = Pattern().Match(s);
        if (!match.Success)
        {
            throw new FormatException();
        }
        var coolLooking = int.Parse(match.Groups["CoolLooking"].Value);
        var musical = int.Parse(match.Groups["Musical"].Value);
        var aerodynamic = int.Parse(match.Groups["Aerodynamic"].Value);
        var shiny = int.Parse(match.Groups["Shiny"].Value);
        return new Part(coolLooking, musical, aerodynamic, shiny);
    }
}