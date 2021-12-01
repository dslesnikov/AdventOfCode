using System.Threading.Tasks;

namespace AdventOfCode.Day3
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private string[] _lines;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 3;
        
        public async Task InitializeAsync()
        {
            _lines = await _reader.ReadLinesAsync();
        }

        public string PartOne()
        {
            var result = NumberOfTrees(3, 1);
            return result.ToString();
        }

        public string PartTwo()
        {
            var result = NumberOfTrees(1, 1) *
                         NumberOfTrees(3, 1) *
                         NumberOfTrees(5, 1) *
                         NumberOfTrees(7, 1) *
                         NumberOfTrees(1, 2);
            return result.ToString();
        }

        private ulong NumberOfTrees(int xStep, int yStep)
        {
            var x = 0;
            var result = 0ul;
            for (var y = yStep; y < _lines.Length; y += yStep)
            {
                x = (x + xStep) % _lines[y].Length;
                if (_lines[y][x] == '#')
                {
                    result++;
                }
            }
            return result;
        }
    }
}