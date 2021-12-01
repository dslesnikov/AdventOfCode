using System;

namespace AdventOfCode.Day8
{
    public abstract record Command
    {
        public static Command Parse(string line)
        {
            var sign = line[4] == '+' ? 1 : -1;
            var number = int.Parse(line[5..]);
            return line[..3] switch
            {
                "nop" => new NopCommand(number * sign),
                "jmp" => new JmpCommand(number * sign),
                "acc" => new AccCommand(number * sign),
                _ => throw new NotSupportedException()
            };
        }
    }
}