using Solution = AdventOfCode._25_Snowverload.Solution;

var input = await File.ReadAllTextAsync($"input/{Solution.Day:00}.txt");
var solution = Solution.Parse(input);
Console.WriteLine(solution.Part1());
Console.WriteLine(solution.Part2());