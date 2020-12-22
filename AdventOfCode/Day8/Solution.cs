using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Day8
{
    public class Solution : IDaySolution
    {
        private Command[] _commands;
        private readonly IInputReader _reader;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 8;
        
        public async Task InitializeAsync()
        {
            _commands = await _reader.ReadAsync(Command.Parse);
        }

        public string PartOne()
        {
            var result = ExecuteProgram(_commands);
            return result.Accumulator.ToString();
        }

        public string PartTwo()
        { 
            for (var i = 0; i < _commands.Length; i++)
            {
                var current = _commands[i];
                TerminationState result;
                switch (_commands[i])
                {
                    case JmpCommand jmpCommand:
                        _commands[i] = new NopCommand(jmpCommand.Address);
                        result = ExecuteProgram(_commands);
                        if (result.InfiniteLoop)
                        {
                            _commands[i] = current;
                        }
                        else
                        {
                            return result.Accumulator.ToString();
                        }
                        break;
                    case NopCommand nopCommand:
                        _commands[i] = new NopCommand(nopCommand.Number);
                        result = ExecuteProgram(_commands);
                        if (result.InfiniteLoop)
                        {
                            _commands[i] = current;
                        }
                        else
                        {
                            return result.Accumulator.ToString();
                        }
                        break;
                }
            }
            return string.Empty;
        }
        
        private TerminationState ExecuteProgram(Command[] commands)
        {
            var accumulator = 0;
            var index = 0;
            var visited = new HashSet<int>();
            while (index < commands.Length)
            {
                if (visited.Contains(index))
                {
                    return new TerminationState(accumulator, true);
                }
                visited.Add(index);
                var command = commands[index];
                switch (command)
                {
                    case AccCommand accCommand:
                        accumulator += accCommand.Value;
                        index++;
                        continue;
                    case JmpCommand jmpCommand:
                        index += jmpCommand.Address;
                        continue;
                    case NopCommand:
                        index++;
                        continue;
                    default:
                        throw new NotSupportedException();
                }
            }
            return new TerminationState(accumulator, false);
        }
    }
}