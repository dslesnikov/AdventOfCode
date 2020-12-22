using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day7
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private BagRule[] _rules;
        private Dictionary<string, BagRule> _contents;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 7;
        
        public async Task InitializeAsync()
        {
            _rules = await _reader.ReadAsync(BagRule.Parse);
            _contents = _rules.ToDictionary(r => r.Container);
        }

        public string PartOne()
        {
            return _rules.Count(rule => CanContainColor(rule.Container, "shiny gold")).ToString();
        }

        public string PartTwo()
        {
            return CountNumberOfBags("shiny gold").ToString();
        }

        private int CountNumberOfBags(string color)
        {
            return _contents[color].Contents.Sum(pair => pair.Count + pair.Count * CountNumberOfBags(pair.Name));
        }

        private bool CanContainColor(string bagColor, string targetColor)
        {
            return _contents[bagColor].Contents.Any(c => c.Name == targetColor || CanContainColor(c.Name, targetColor));
        }
    }
}