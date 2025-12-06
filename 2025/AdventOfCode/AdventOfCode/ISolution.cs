namespace AdventOfCode;

public interface ISolution
{
    static abstract int Day { get; }

    string SolvePartOne();

    string SolvePartTwo();
}
