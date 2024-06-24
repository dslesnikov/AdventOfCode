namespace AdventOfCode._19_Aplenty;

public abstract record CheckResult
{
    public record Failed : CheckResult
    {
        public static Failed Instance { get; } = new();
    }

    public record Accept : CheckResult
    {
        public static Accept Instance { get; } = new();
    }

    public record Reject : CheckResult
    {
        public static Reject Instance { get; } = new();
    }

    public record Transfer(string Name) : CheckResult;
}