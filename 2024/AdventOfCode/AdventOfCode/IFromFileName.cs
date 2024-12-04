namespace AdventOfCode;

public interface IFromFileName<T> : ISolution<T>
    where T : ISolution<T>
{
    static abstract T Create(string fileName);
}