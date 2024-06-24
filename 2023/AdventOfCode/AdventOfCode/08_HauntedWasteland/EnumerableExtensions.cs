using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._08_HauntedWasteland;

public static class EnumerableExtensions
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    public static IEnumerable<T> Loop<T>(this IEnumerable<T> source)
    {
        while (true)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}