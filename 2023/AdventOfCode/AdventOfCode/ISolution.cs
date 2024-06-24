namespace AdventOfCode;

public interface ISimpleParsable<T> where T : ISimpleParsable<T>
{
    static abstract T Parse(string s);
}

public interface ISolution<TSolution> : ISimpleParsable<TSolution>
    where TSolution : ISimpleParsable<TSolution>
{
    static abstract int Day { get; }

    string Part1();

    string Part2();
}