using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Day13
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private int _startTimestamp;
        private (int Id, int SkipCount)[] _buses;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 13;
        
        public async Task InitializeAsync()
        {
            var lines = await _reader.ReadLinesAsync();
            _startTimestamp = int.Parse(lines[0]);
            var items = lines[1].Split(',');
            var buses = new List<(int Id, int SkipCount)>();
            var skip = 0;
            foreach (var scheduleItem in items)
            {
                if (int.TryParse(scheduleItem, out var busId))
                {
                    buses.Add((busId, skip));
                }
                skip++;
            }
            _buses = buses.ToArray();
        }

        public string PartOne()
        {
            for (var timestamp = _startTimestamp;; timestamp++)
            {
                var divisor = _buses.FirstOrDefault(id => timestamp % id.Id == 0).Id;
                if (divisor != 0)
                {
                    return (divisor * (timestamp - _startTimestamp)).ToString();
                }
            }
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Chinese_remainder_theorem
        /// </remarks>
        public string PartTwo()
        {
            var multiple = _buses.Aggregate<(int Id, int), BigInteger, BigInteger>(1, (acc, bus) => bus.Id * acc, acc => acc);
            var result = new BigInteger(0);
            foreach (var (id, skipCount) in _buses)
            {
                if (skipCount == 0)
                {
                    continue;
                }
                var remainder = (id - skipCount) % id;
                remainder = remainder < 0 ? id + remainder : remainder;
                var m = multiple / id;
                var mInverse = ModularInverse(m, id);
                result += (remainder * m * mInverse) % multiple;
            }
            return (result % multiple).ToString();
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Fermat%27s_little_theorem
        /// </remarks>
        private BigInteger ModularInverse(BigInteger value, int module)
        {
            return BigInteger.ModPow(value, module - 2, module);
        }
    }
}