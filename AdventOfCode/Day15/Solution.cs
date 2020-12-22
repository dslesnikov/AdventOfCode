using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Day15
{
    public class Solution : IDaySolution
    {
        private static readonly int[] InitialNumbers = {20, 9, 11, 0, 1, 2};
        
        public int DayNumber => 15;

        public Task InitializeAsync() => Task.CompletedTask;
        
        public string PartOne()
        {
            var result = RunGame(2020);
            return result.ToString();
        }

        public string PartTwo()
        {
            var result = RunGame(30000000);
            return result.ToString();
        }

        private int RunGame(int turns)
        {
            var lastPosition = new Dictionary<int, int>();
            var i = 0;
            for (; i < InitialNumbers.Length; i++)
            {
                lastPosition[InitialNumbers[i]] = i;
            }
            lastPosition[InitialNumbers[^1]] = -1;
            var previous = InitialNumbers[^1];
            for (; i < turns; i++)
            {
                var result = lastPosition[previous] == -1
                    ? 0
                    : i - 1 - lastPosition[previous];
                lastPosition[previous] = i - 1;
                if (!lastPosition.ContainsKey(result))
                {
                    lastPosition[result] = -1;
                }
                previous = result;
            }
            return previous;
        }
    }
}