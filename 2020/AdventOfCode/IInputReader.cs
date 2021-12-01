using System;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public interface IInputReader
    {
        Task<string> ReadTextAsync();
        
        Task<string[]> ReadLinesAsync();

        Task<T[]> ReadAsync<T>(Func<string, T> converter);
    }
}