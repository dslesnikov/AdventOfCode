using System;
using System.Threading.Tasks;

namespace AdventOfCode.Day9
{
    public class Solution : IDaySolution
    {
        private ulong[] _numbers;
        private readonly IInputReader _reader;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 9;
        
        public async Task InitializeAsync()
        {
            _numbers = await _reader.ReadAsync(ulong.Parse);
        }

        public string PartOne()
        {
            for (var i = 25; i < _numbers.Length; i++)
            {
                var currentNumber = _numbers[i];
                var isValid = false;
                for (var j = i - 25; j < i - 1; j++)
                {
                    var first = _numbers[j];
                    for (var k = j + 1; k < i; k++)
                    {
                        var second = _numbers[k];
                        if (first + second == currentNumber)
                        {
                            isValid = true;
                        }
                    }
                }
                if (!isValid)
                {
                    return currentNumber.ToString();
                }
            }
            return string.Empty;
        }

        public string PartTwo()
        {
            var invalidNumber = ulong.Parse(PartOne());
            var rangeIndexes = FindContiguousSet(invalidNumber);
            var range = _numbers[rangeIndexes.Item1..rangeIndexes.Item2];
            Array.Sort(range);
            var result = range[0] + range[^1];
            return result.ToString();
        }

        private (int,int) FindContiguousSet(ulong target)
        {
            var currentSum = _numbers[0];
            var startIndex = 0;
            for (var i = 1; i < _numbers.Length; i++)
            {
                var number = _numbers[i];
                while (currentSum + number > target)
                {
                    currentSum -= _numbers[startIndex++];
                }
                if (currentSum + number == target)
                {
                    return (startIndex, i);
                }
                currentSum += number;
            }
            return (-1, -1);
        }
    }
}