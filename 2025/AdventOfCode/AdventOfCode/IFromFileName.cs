namespace AdventOfCode;

public interface IFromFileName<out T>
{
    static abstract T Create(string fileName);
}
