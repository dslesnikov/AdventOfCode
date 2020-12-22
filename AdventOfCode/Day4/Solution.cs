using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Day4
{
    public class Solution : IDaySolution
    {
        private static readonly string[] RequiredFields = {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
        private static readonly string[] EyeColors = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

        private readonly IInputReader _reader;
        private Dictionary<string, string>[] _passports;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 4;
        
        public async Task InitializeAsync()
        {
            var text = await _reader.ReadTextAsync();
            _passports = text.Split("\n\n")
                .Select(passport => Regex.Split(passport, @"\s+").Where(f => !string.IsNullOrEmpty(f))
                    .Select(f => f.Split(':'))
                    .ToDictionary(split => split[0], split => split[1]))
                .ToArray();
        }

        public string PartOne()
        {
            var result = _passports.Count(p => RequiredFields.All(p.ContainsKey));
            return result.ToString();
        }

        public string PartTwo()
        {
            var result = _passports.Count(Validate);
            return result.ToString();
        }

        private bool Validate(Dictionary<string, string> passport)
        {
            var hasRequiredFields = RequiredFields.All(passport.ContainsKey);
            if (!hasRequiredFields)
            {
                return false;
            }
            return passport.All(pair => pair.Key switch
            {
                "byr" => ValidateYear(pair.Value, 1920, 2002),
                "iyr" => ValidateYear(pair.Value, 2010, 2020),
                "eyr" => ValidateYear(pair.Value, 2020, 2030),
                "hcl" => Regex.IsMatch(pair.Value, "^#[0-9a-f]{6}$"),
                "ecl" => EyeColors.Contains(pair.Value),
                "pid" => Regex.IsMatch(pair.Value, "^\\d{9}$"),
                "hgt" => ValidateHeight(pair.Value),
                "cid" => true,
                _ => throw new NotSupportedException()
            });
        }

        private bool ValidateYear(string value, int min, int max)
        {
            var match = Regex.IsMatch(value, @"^\d{4}$");
            if (!match)
            {
                return false;
            }
            var parsed = int.Parse(value);
            return parsed >= min && parsed <= max;
        }

        private bool ValidateHeight(string value)
        {
            var match = Regex.Match(value, @"^(?<number>\d+)(?<unit>cm|in)$");
            if (!match.Success)
            {
                return false;
            }
            var parsed = int.Parse(match.Groups["number"].Value);
            return match.Groups["unit"].Value switch
            {
                "cm" => parsed >= 150 && parsed <= 193,
                "in" => parsed >= 59 && parsed <= 76,
                _ => throw new NotSupportedException()
            };
        }
    }
}