namespace AdventOfCode;

public interface IFromLines<T, TParsed> : IFromFileName<T>
    where T : ISolution<T>, IFromLines<T, TParsed>
{
    static abstract TParsed ParseLine(ReadOnlySpan<char> line);

    static abstract T FromParsed(IReadOnlyList<TParsed> entries);

    static T IFromFileName<T>.Create(string fileName)
    {
        var text = File.ReadAllText(fileName);
        var input = text.AsSpan();
        var split = input.Split('\n');
        var entries = new List<TParsed>();
        foreach (var range in split)
        {
            var parsed = T.ParseLine(input[range.Start..range.End]);
            entries.Add(parsed);
        }
        return T.FromParsed(entries);
    }
}