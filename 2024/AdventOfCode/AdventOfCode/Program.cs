using AdventOfCode;
using AdventOfCode.Day21KeypadConundrum;

var solution = Create<Solution>();
var partOne = solution.SolvePartOne();
Console.WriteLine($"Part One: {partOne}");
var partTwo = solution.SolvePartTwo();
Console.WriteLine($"Part Two: {partTwo}");
return;

static ISolution<TSolution> Create<TSolution>()
    where TSolution : IFromFileName<TSolution>
{
    return TSolution.Create($"input/{TSolution.Day:00}.txt");
}
