using System.Collections.Generic;

namespace AdventOfCode.Day23
{
    public static class LinkedListExtensions
    {
        public static LinkedListNode<int> GetNextCircular(this LinkedListNode<int> item)
        {
            return item.Next ?? item.List!.First;
        }
    }
}