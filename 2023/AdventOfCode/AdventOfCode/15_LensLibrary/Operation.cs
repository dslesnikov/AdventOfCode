namespace AdventOfCode._15_LensLibrary;

public abstract record Operation(string Label) : ISimpleParsable<Operation>
{
    public static Operation Parse(string s)
    {
        if (s[^1] == '-')
        {
            return new Remove(s[..^1]);
        }
        var split = s.Split('=');
        return new Upsert(split[0], int.Parse(split[1]));
    }

    public abstract string Instruction { get; }
}