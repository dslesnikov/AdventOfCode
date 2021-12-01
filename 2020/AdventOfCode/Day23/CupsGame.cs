using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Day23
{
    public class CupsGame
    {
        private readonly Dictionary<int, LinkedListNode<int>> _nodeLookup;
        private readonly LinkedList<int> _items;
        private readonly int _cupsCount;
        private LinkedListNode<int> _currentCup;

        public CupsGame(IEnumerable<int> cups)
        {
            _items = new LinkedList<int>();
            _nodeLookup = new Dictionary<int, LinkedListNode<int>>();
            foreach (var number in cups)
            {
                var node = new LinkedListNode<int>(number);
                _items.AddLast(node);
                _nodeLookup[number] = node;
            }
            _cupsCount = _nodeLookup.Count;
            _currentCup = _items.First;
        }

        public IEnumerable<int> GetItemsSequence(int startItem)
        {
            var start = _nodeLookup[startItem];
            return new GameStateEnumerable(start);
        }

        public void RunFor(int rounds)
        {
            for (var i = 0; i < rounds; i++)
            {
                TickOnce();
            }
        }

        private void TickOnce()
        {
            Span<int> cupsToMove = stackalloc int[3];
            var first = _currentCup.GetNextCircular();
            cupsToMove[0] = first.Value;
            var second = first.GetNextCircular();
            cupsToMove[1] = second.Value;
            var third = second.GetNextCircular();
            cupsToMove[2] = third.Value;
            _items.Remove(third);
            _items.Remove(second);
            _items.Remove(first);
            var destinationCupValue = SelectDestination(cupsToMove);
            var destination = _nodeLookup[destinationCupValue];
            _items.AddAfter(destination, first);
            _items.AddAfter(first, second);
            _items.AddAfter(second, third);
            _currentCup = _currentCup.GetNextCircular();
        }

        private int SelectDestination(Span<int> toMove)
        {
            var target = _currentCup.Value - 1 == 0  ? _cupsCount : _currentCup.Value - 1;
            while (toMove.Contains(target))
            {
                target = target <= 1 ? _cupsCount : target - 1;
            }
            return target;
        }
        
        private readonly struct GameStateEnumerable : IEnumerable<int>
        {
            private readonly LinkedListNode<int> _start;
            
            public GameStateEnumerable(LinkedListNode<int> startNode)
            {
                _start = startNode;
            }
            
            public IEnumerator<int> GetEnumerator()
            {
                return new Enumerator(_start);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        
        private struct Enumerator : IEnumerator<int>
        {
            private LinkedListNode<int> _current;
            
            public Enumerator(LinkedListNode<int> start)
            {
                _current = start.Previous;
            }
            
            public bool MoveNext()
            {
                _current = _current.GetNextCircular();
                return true;
            }

            public void Reset()
            {
                _current = _current.List!.First;
            }

            public int Current => _current.Value;

            object IEnumerator.Current => Current;

            public void Dispose() { }
        }
    }
}