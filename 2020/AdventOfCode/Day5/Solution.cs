using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day5
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private int[] _seats;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 5;
        
        public async Task InitializeAsync()
        {
            var lines = await _reader.ReadLinesAsync();
            _seats = new int[lines.Length];
            var counter = 0;
            foreach (var seat in lines)
            {
                var row = FoldBinaryString(seat.Take(7), (0, 'F'), (127, 'B'));
                var column = FoldBinaryString(seat.Skip(7), (0, 'L'), (7, 'R'));
                _seats[counter++] = row * 8 + column;
            }
            Array.Sort(_seats);
        }

        public string PartOne()
        {
            var result = _seats[^1];
            return result.ToString();
        }

        public string PartTwo()
        {
            for (var i = 1; i < _seats.Length - 2; i++)
            {
                if (_seats[i + 1] - _seats[i] > 1)
                {
                    var result = ((ulong)_seats[i + 1] + (ulong)_seats[i]) / 2;
                    return result.ToString();
                }
            }
            return string.Empty;
        }

        private int FoldBinaryString(IEnumerable<char> input, (int Value, char Char) low, (int Value, char Char) high)
        {
            foreach (var @char in input)
            {
                if (@char == low.Char)
                {
                    high.Value = (high.Value + low.Value) / 2;
                    continue;
                }
                if (@char == high.Char)
                {
                    low.Value = (high.Value + low.Value) / 2 + 1;
                }
            }
            return low.Value;
        }
    }
}