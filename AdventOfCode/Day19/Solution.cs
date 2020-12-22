using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Day19
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private string[] _messages;
        private Dictionary<int, MessageRule> _rules;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 19;
        
        public async Task InitializeAsync()
        {
            var text = await _reader.ReadTextAsync();
            var split = text.Split("\n\n");
            _messages = split[1].Split('\n');
            _rules = split[0]
                .Split('\n')
                .Select(line => MessageRule.Parse(line.Trim()))
                .ToDictionary(r => r.Number);
        }

        public string PartOne()
        {
            var pattern = GetRulePattern(_rules);
            var regex = new Regex($"^{pattern}$");
            var count = _messages.Count(message => regex.IsMatch(message));
            return count.ToString();
        }

        public string PartTwo()
        {
            _rules[0].Pattern = null;
            _rules[8] = new MessageRule
            {
                Number = 8,
                DependsOn = new[] {42},
                DisjunctionSequences = new[]
                {
                    new[] {42},
                    new[] {42, 42},
                    new[] {42, 42, 42},
                    new[] {42, 42, 42, 42},
                    new[] {42, 42, 42, 42, 42}
                }
            };
            _rules[11] = new MessageRule
            {
                Number = 11,
                DependsOn = new[] {42, 31},
                DisjunctionSequences = new[]
                {
                    new[] {42, 31},
                    new[] {42, 42, 31, 31},
                    new[] {42, 42, 42, 31, 31, 31},
                    new[] {42, 42, 42, 42, 31, 31, 31, 31},
                    new[] {42, 42, 42, 42, 42, 31, 31, 31, 31, 31}
                }
            };
            var pattern = GetRulePattern(_rules);
            var regex = new Regex($"^{pattern}$");
            var count = _messages.Count(message => regex.IsMatch(message));
            return count.ToString();
        }

        private string GetRulePattern(Dictionary<int, MessageRule> rules)
        {
            var simple = rules.Values.Where(r => r.IsComputed).Select(r => r.Number).ToArray();
            var completed = new HashSet<int>(simple);
            while (completed.Count != rules.Count)
            {
                var canBeComputed = rules
                    .Values
                    .Where(r => !completed.Contains(r.Number) && r.DependsOn.All(dependency => completed.Contains(dependency)))
                    .ToArray();
                foreach (var messageRule in canBeComputed)
                {
                    var pattern = string.Join("|",
                        messageRule.DisjunctionSequences
                            .Select(sequence => $"({string.Join(string.Empty, sequence.Select(ruleNumber => rules[ruleNumber].Pattern))})"));
                    messageRule.Pattern = $"({pattern})";
                    completed.Add(messageRule.Number);
                }
            }
            return rules[0].Pattern;
        }
    }
}