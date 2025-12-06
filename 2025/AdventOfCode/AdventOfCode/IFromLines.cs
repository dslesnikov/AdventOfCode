namespace AdventOfCode;

public interface IFromLines<T, TParsed> : IFromFileName<T>
    where T : IFromLines<T, TParsed>
{
    static abstract TParsed ParseLine(string line);

    static abstract T FromParsed(IReadOnlyList<TParsed> entries);

    static T IFromFileName<T>.Create(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        var entries = lines.Select(T.ParseLine).ToList();
        return T.FromParsed(entries);
    }
}
