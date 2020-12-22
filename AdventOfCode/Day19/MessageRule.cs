using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day19
{
    public class MessageRule
    {
        private static readonly Regex RuleDependencyRegex = new(@"((?<RuleSequence>( \d+)+)( \|)?)+", RegexOptions.Compiled);
        private static readonly Regex SimpleRuleRegex = new("\"(?<Character>\\w)\"", RegexOptions.Compiled);

        public int Number { get; init; }
        
        public int[] DependsOn { get; init; }
        
        public string RuleValue { get; init; }
        
        public int[][] DisjunctionSequences { get; init; }
        
        public string Pattern { get; set; }

        public bool IsComputed => !string.IsNullOrEmpty(Pattern);

        public static MessageRule Parse(string line)
        {
            var split = line.Split(':');
            var number = int.Parse(split[0]);
            var value = split[1].Trim();
            var ruleDependencyMatch = RuleDependencyRegex.Match(line);
            if (ruleDependencyMatch.Success)
            {
                var sequences = ruleDependencyMatch
                    .Groups["RuleSequence"]
                    .Captures
                    .Select(capture => capture.Value.Trim().Split(' ').Select(int.Parse).ToArray())
                    .ToArray();
                return new MessageRule
                {
                    Number = number,
                    DisjunctionSequences = sequences,
                    DependsOn = sequences.SelectMany(x => x).ToArray(),
                    RuleValue = value
                };
            }
            var simpleMatch = SimpleRuleRegex.Match(line);
            var character = simpleMatch.Groups["Character"].Value;
            return new MessageRule
            {
                Number = number,
                DisjunctionSequences = Array.Empty<int[]>(),
                Pattern = character,
                DependsOn = Array.Empty<int>(),
                RuleValue = value
            };
        }
    }
}