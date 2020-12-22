using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class InputReader : IInputReader
    {
        private readonly int _day;

        public InputReader(Func<int> daySelector)
        {
            _day = daySelector();
        }

        public async Task<string> ReadTextAsync()
        {
            var text = (await File.ReadAllTextAsync($"Day{_day}/input.txt")).Trim();
            return text;
        }

        public async Task<string[]> ReadLinesAsync()
        {
            var text = await ReadTextAsync();
            return text.Split('\n');
        }

        public async Task<T[]> ReadAsync<T>(Func<string, T> converter)
        {
            var lines = await ReadLinesAsync();
            return lines.Select(converter).ToArray();
        }
    }
}