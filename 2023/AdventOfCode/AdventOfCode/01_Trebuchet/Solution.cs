using System.Text.RegularExpressions;

namespace AdventOfCode._01_Trebuchet;

public partial class Solution(IReadOnlyList<string> lines) : ISolution<Solution>
{
    public static int Day => 1;

    public static Solution Parse(string s)
    {
        return new Solution(s.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    public string Part1()
    {
        var numbers = lines
            .Select(line =>
            {
                var firstDigit = line.First(char.IsDigit);
                var lastDigit = line.Last(char.IsDigit);
                var result = (firstDigit - '0') * 10 + (lastDigit - '0');
                return result;
            });
        return numbers.Sum().ToString();
    }

    public string Part2()
    {
        var numbers = lines
            .Select(line =>
            {
                var firstMatch = Numbers.Regex().Match(line);
                var firstValue = Numbers.ToInt(firstMatch.Value);
                var secondMatch = Numbers.RegexReversed().Match(line);
                var secondValue = Numbers.ToInt(secondMatch.Value);
                var result = firstValue * 10 + secondValue;
                return result;
            });
        return numbers.Sum().ToString();
    }

    private static partial class Numbers
    {
        private const string Pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

        [GeneratedRegex(Pattern)]
        public static partial Regex Regex();

        [GeneratedRegex(Pattern, RegexOptions.RightToLeft)]
        public static partial Regex RegexReversed();

        public static int ToInt(string number)
        {
            return number switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                _ => int.Parse(number)
            };
        }
    }
}