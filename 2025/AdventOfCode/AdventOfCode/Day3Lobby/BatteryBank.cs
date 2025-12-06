namespace AdventOfCode.Day3Lobby;

public record BatteryBank(IReadOnlyList<int> Values)
{
    public static BatteryBank FromString(string line)
    {
        return new BatteryBank(line.Select(c => c - '0').ToArray());
    }
}