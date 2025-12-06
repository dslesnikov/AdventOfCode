using AdventOfCode;
using AdventOfCode.Day6TrashCompactor;

var solution = Create<Solution>();

var part1 = solution.SolvePartOne();
Console.WriteLine($"Part 1: {part1}");

var part2 = solution.SolvePartTwo();
Console.WriteLine($"Part 2: {part2}");

return;

static ISolution Create<TSolution>()
    where TSolution : IFromFileName<TSolution>, ISolution
{
    return TSolution.Create($"input/{TSolution.Day:00}.txt");
}
