namespace AdventOfCode;

public interface IFromText<T> : IFromFileName<T>
    where T : ISolution<T>, IFromText<T>
{
    static abstract T FromText(string text);

    static T IFromFileName<T>.Create(string fileName)
    {
        var text = File.ReadAllText(fileName);
        return T.FromText(text);
    }
}