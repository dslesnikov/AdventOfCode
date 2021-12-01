using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day10
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private int[] _numbers;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 10;
        
        public async Task InitializeAsync()
        {
            _numbers = await _reader.ReadAsync(int.Parse);
            Array.Sort(_numbers);
        }

        public string PartOne()
        {
            var oneJolts = 0ul;
            var threeJolts = 1ul;
            for (var i = 0; i < _numbers.Length; i++)
            {
                var previous = i > 0 ? _numbers[i - 1] : 0;
                switch (_numbers[i] - previous)
                {
                    case 1:
                        oneJolts++;
                        break;
                    case 3:
                        threeJolts++;
                        break;
                }
            }
            return (oneJolts * threeJolts).ToString();
        }

        public string PartTwo()
        {
            var joltage = _numbers.Max();
            var numberOfWays = new ulong[joltage + 1];
            numberOfWays[0] = 1;
            foreach (var current in _numbers)
            {
                numberOfWays[current] =
                    current > 0
                        ? (current > 1
                            ? (current > 2
                                ? numberOfWays[current - 3] + numberOfWays[current - 2] + numberOfWays[current - 1]
                                : numberOfWays[current - 2] + numberOfWays[current - 1])
                            : numberOfWays[current - 1])
                        : 0;
            }
            return numberOfWays[joltage].ToString();
        }
    }
}