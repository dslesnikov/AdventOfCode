using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day14
{
    public abstract record Command
    {
        private static readonly Regex MaskRegex = new("^mask = (?<mask>[X10]{36})$");
        private static readonly Regex MemorySetRegex = new(@"^mem\[(?<address>\d+)\] = (?<value>\d+)$");
        
        public static Command Parse(string line)
        {
            var maskMatch = MaskRegex.Match(line);
            if (maskMatch.Success)
            {
                var maskValue = maskMatch.Groups["mask"].Value;
                var unset = 0ul;
                var mask = 0ul;
                for (var i = 0; i < maskValue.Length; i++)
                {
                    unset |= (maskValue[maskValue.Length - 1 - i] == 'X' ? 1ul << i : 0);
                    mask |= (maskValue[maskValue.Length - 1 - i] == '1' ? 1ul << i : 0);
                }
                return new MaskSetCommand(unset, mask);
            }
            var memorySetMatch = MemorySetRegex.Match(line);
            var address = int.Parse(memorySetMatch.Groups["address"].Value);
            var value = int.Parse(memorySetMatch.Groups["value"].Value);
            return new MemorySetCommand(address, value);
        }
    }
}