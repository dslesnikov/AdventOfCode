using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Day6
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private string[] _groups;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 6;
        public async Task InitializeAsync()
        {
            var text = await _reader.ReadTextAsync();
            _groups = text.Split("\n\n");
        }

        public string PartOne()
        {
            var result = _groups
                .Select(group => group.Split('\n')
                    .Select(entry => entry.Aggregate(0, (alphabet, answer) => alphabet | 1 << (answer - 'a')))
                    .Aggregate(0, (accumulator, alphabet) => accumulator | alphabet))
                .Sum(alphabet => BitOperations.PopCount((uint)alphabet));
            return result.ToString();
        }

        public string PartTwo()
        {
            var result = _groups
                .Select(group => group.Split('\n')
                    .Select(entry => entry.Aggregate(0, (alphabet, answer) => alphabet | 1 << (answer - 'a')))
                    .Aggregate(~0, (accumulator, alphabet) => accumulator & alphabet))
                .Sum(alphabet => BitOperations.PopCount((uint)alphabet));
            return result.ToString();
        }
    }
}