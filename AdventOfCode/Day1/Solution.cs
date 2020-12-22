using System;
using System.Threading.Tasks;

namespace AdventOfCode.Day1
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private int[] _numbers;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 1;
        
        public async Task InitializeAsync()
        {
            _numbers = await _reader.ReadAsync(int.Parse);
            Array.Sort(_numbers);
        }

        public string PartOne()
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                var target = 2020 - _numbers[i];
                var found = Array.BinarySearch(_numbers, i + 1, _numbers.Length - i - 1, target);
                if (found >= 0)
                {
                    var result = _numbers[i] * _numbers[found];
                    return result.ToString();
                }
            }
            return string.Empty;
        }

        public string PartTwo()
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                for (var j = i + 1; j < _numbers.Length; j++)
                {
                    var sum = _numbers[i] + _numbers[j];
                    var target = 2020 - sum;
                    var found = Array.BinarySearch(_numbers, j + 1, _numbers.Length - j - 1, target);
                    if (found >= 0)
                    {
                        var result = _numbers[i] * _numbers[found] * _numbers[j];
                        return result.ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}