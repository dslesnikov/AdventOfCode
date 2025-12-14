namespace AdventOfCode.Day11Reactor;

public record Connection(string From, IReadOnlyList<string> To)
{
    public static Connection Parse(string line)
    {
        var split = line.Split(':', StringSplitOptions.TrimEntries);
        var from = split[0].Trim();
        var to = split[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        return new Connection(from, to);
    }
}