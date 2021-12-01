using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day14
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private Command[] _commands;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 14;
        
        public async Task InitializeAsync()
        {
            _commands = await _reader.ReadAsync(Command.Parse);
        }

        public string PartOne()
        {
            var memory = new Dictionary<int, ulong>();
            var currentFree = 0ul;
            var currentMask = 0ul;
            foreach (var command in _commands)
            {
                switch (command)
                {
                    case MaskSetCommand maskSetCommand:
                        currentFree = maskSetCommand.Unset;
                        currentMask = maskSetCommand.Mask;
                        break;
                    case MemorySetCommand memorySetCommand:
                        var value = (ulong)memorySetCommand.Value;
                        var result = value & currentFree;
                        result |= currentMask;
                        memory[memorySetCommand.Address] = result;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return memory.Values.Aggregate((acc, v) => acc + v).ToString();
        }

        public string PartTwo()
        {
            var memory = new Dictionary<ulong, int>();
            var currentFloating = 0ul;
            var currentMask = 0ul;
            foreach (var command in _commands)
            {
                switch (command)
                {
                    case MaskSetCommand maskSetCommand:
                        currentFloating = maskSetCommand.Unset;
                        currentMask = maskSetCommand.Mask;
                        break;
                    case MemorySetCommand memorySetCommand:
                        var addressVariations = SubstituteFloatingBits(currentFloating, currentMask, memorySetCommand.Address);
                        foreach (var memoryAddress in addressVariations)
                        {
                            memory[memoryAddress] = memorySetCommand.Value;
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return memory.Values.Aggregate(0ul, (acc, i) => acc + (ulong) i, x => x).ToString();
        }

        private static ulong[] SubstituteFloatingBits(ulong floating, ulong mask, int value)
        {
            var memValue = (ulong) value;
            var appliedMask = memValue | mask;
            appliedMask &= ~floating;
            var variations = new HashSet<ulong>();
            FillFreeBitsPermutations(floating, appliedMask, variations);
            return variations.ToArray();
        }

        private static void FillFreeBitsPermutations(ulong floating, ulong address, HashSet<ulong> result)
        {
            if (floating == 0)
            {
                return;
            }
            var firstFreeIndex = -1;
            var index = 0;
            while (firstFreeIndex == -1)
            {
                firstFreeIndex = ((floating >> index) & 1ul) == 1 ? index : -1;
                index++;
            }
            var newFloating = floating & ~(1ul << firstFreeIndex);
            var zero = address & ~(1ul << firstFreeIndex);
            result.Add(zero);
            FillFreeBitsPermutations(newFloating, zero, result);
            var one = address | (1ul << firstFreeIndex);
            result.Add(one);
            FillFreeBitsPermutations(newFloating, one, result);
        }
    }
}