using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day23
{
    public class Solution : IDaySolution
    {
        private readonly int[] _input = {9, 6, 3, 2, 7, 5, 4, 8, 1};
        
        public int DayNumber => 23;
        
        public Task InitializeAsync() => Task.CompletedTask;

        public string PartOne()
        {
            var game = new CupsGame(_input);
            game.RunFor(100);
            var result = string.Join(string.Empty, game.GetItemsSequence(1).Skip(1).Take(_input.Length - 1));
            return result;
        }

        public string PartTwo()
        {
            var numbers = _input.Concat(Enumerable.Range(10, 1_000_000 - 9));
            var game = new CupsGame(numbers);
            game.RunFor(10_000_000);
            var items = game.GetItemsSequence(1).Skip(1).Take(2).ToArray();
            var result = (ulong)items[0] * (ulong)items[1];
            return result.ToString();
        }
    }
}