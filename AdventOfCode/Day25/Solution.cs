using System;
using System.Threading.Tasks;

namespace AdventOfCode.Day25
{
    public class Solution : IDaySolution
    {
        private readonly int _cardPublicKey = 4542595;
        private readonly int _doorPublicKey = 2959251;
        
        public int DayNumber => 25;
        
        public Task InitializeAsync() => Task.CompletedTask;

        public string PartOne()
        {
            var doorLoopSize = GuessLoopSize(7, _doorPublicKey);
            var cardLoopSize = GuessLoopSize(7, _cardPublicKey);
            var doorKey = CalculateEncryptionKey(_cardPublicKey, doorLoopSize);
            var cardKey = CalculateEncryptionKey(_doorPublicKey, cardLoopSize);
            if (doorKey != cardKey)
            {
                throw new InvalidOperationException();
            }
            return doorKey.ToString();
        }

        public string PartTwo()
        {
            return string.Empty;
        }

        private static int GuessLoopSize(int subjectNumber, int publicKey)
        {
            int result;
            var value = 1ul;
            for (var loopSize = 1;; loopSize++)
            {
                value *= (ulong)subjectNumber;
                value %= 20201227;
                if ((int)value == publicKey)
                {
                    result = loopSize;
                    break;
                }
            }
            return result;
        }

        private static int CalculateEncryptionKey(int subjectNumber, int loopSize)
        {
            var value = 1ul;
            for (var i = 0; i < loopSize; i++)
            {
                value *= (ulong)subjectNumber;
                value %= 20201227;
            }
            return (int)value;
        }
    }
}