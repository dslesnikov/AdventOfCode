namespace AdventOfCode;

public interface ISolution<T> where T: ISolution<T>
{
    static abstract int Day { get; }

    string SolvePartOne();

    string SolvePartTwo();
}