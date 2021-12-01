using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day11
{
    public class Solution :  IDaySolution
    {
        private readonly IInputReader _reader;
        private int[][] _initialState;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 11;
        
        public async Task InitializeAsync()
        {
            var lines = await _reader.ReadLinesAsync();
            _initialState = new int[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                _initialState[i] = new int[line.Length];
                for (var j = 0; j < line.Length; j++)
                {
                    var seat = line[j];
                    _initialState[i][j] = seat switch
                    {
                        'L' => 0,
                        '.' => -1,
                        _ => throw new NotSupportedException()
                    };
                }
            }
        }

        public string PartOne()
        {
            var state = new int[_initialState.Length][];
            var previousState = new int[_initialState.Length][];
            for (var i = 0; i < state.Length; i++)
            {
                state[i] = new int[_initialState[i].Length];
                previousState[i] = new int[_initialState[i].Length];
                Array.Copy(_initialState[i], state[i], state[i].Length);
                Array.Copy(_initialState[i], previousState[i], previousState[i].Length);
            }
            var hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;
                for (var i = 0; i < state.Length; i++)
                {
                    for (var j = 0; j < state[i].Length; j++)
                    {
                        var taken = GetNeighbourTiles(i, j).Count(n => previousState[n.Y][n.X] == 1);
                        if (previousState[i][j] == 0 && taken == 0)
                        {
                            state[i][j] = 1;
                            hasChanged = true;
                        }
                        else if (previousState[i][j] == 1 && taken >= 4)
                        {
                            state[i][j] = 0;
                            hasChanged = true;
                        }
                        else
                        {
                            state[i][j] = previousState[i][j];
                        }
                    }
                }
                var tmp = previousState;
                previousState = state;
                state = tmp;
            }
            var result = state.Sum(row => row.Count(s => s == 1));
            return result.ToString();
        }

        public string PartTwo()
        {
            var state = new int[_initialState.Length][];
            var previousState = new int[_initialState.Length][];
            for (var i = 0; i < state.Length; i++)
            {
                state[i] = new int[_initialState[i].Length];
                previousState[i] = new int[_initialState[i].Length];
                Array.Copy(_initialState[i], state[i], state[i].Length);
                Array.Copy(_initialState[i], previousState[i], previousState[i].Length);
            }
            var hasChanged = true;
            var neighbourCache = new Dictionary<(int, int), (int Y, int X)[]>();
            while (hasChanged)
            {
                hasChanged = false;
                for (var i = 0; i < state.Length; i++)
                {
                    for (var j = 0; j < state[i].Length; j++)
                    {
                        if (!neighbourCache.ContainsKey((i, j)))
                        {
                            neighbourCache[(i, j)] = GetNeighbourSeats(i, j).ToArray();
                        }
                        var taken = neighbourCache[(i,j)].Count(n => previousState[n.Y][n.X] == 1);
                        if (previousState[i][j] == 0 && taken == 0)
                        {
                            state[i][j] = 1;
                            hasChanged = true;
                        }
                        else if (previousState[i][j] == 1 && taken >= 5)
                        {
                            state[i][j] = 0;
                            hasChanged = true;
                        }
                        else
                        {
                            state[i][j] = previousState[i][j];
                        }
                    }
                }
                var tmp = previousState;
                previousState = state;
                state = tmp;
            }
            var result = state.Sum(row => row.Count(s => s == 1));
            return result.ToString();
        }
        
        private IEnumerable<(int Y, int X)> GetNeighbourSeats(int i, int j)
        {
            var start = (i, j);
            var result = new[]
                {
                    TraverseAndFindSeat(start, 1, 0),
                    TraverseAndFindSeat(start, 1, 1),
                    TraverseAndFindSeat(start, 1, -1),
                    TraverseAndFindSeat(start, -1, 0),
                    TraverseAndFindSeat(start, -1, 1),
                    TraverseAndFindSeat(start, -1, -1),
                    TraverseAndFindSeat(start, 0, 1),
                    TraverseAndFindSeat(start, 0, -1)
                }
                .Where(pair => pair.X >= 0)
                .ToArray();
            return result;
        }

        private (int Y, int X) TraverseAndFindSeat((int Y, int X) start, int ystep, int xstep)
        {
            var current = (Y: start.Y + ystep, X: start.X + xstep);
            while (current.Y >= 0 && current.X >= 0 && current.Y < _initialState.Length && current.X < _initialState[0].Length)
            {
                if (_initialState[current.Y][current.X] != -1)
                {
                    return (current.Y, current.X);
                }
                current = (Y: current.Y + ystep, X: current.X + xstep);
            }
            return (-1, -1);
        }
        
        private IEnumerable<(int Y, int X)> GetNeighbourTiles(int i, int j)
        {
            if (j > 0)
            {
                yield return (i, j - 1);
            }
            if (j < _initialState[0].Length - 1)
            {
                yield return (i, j + 1);
            }
            if (i > 0)
            {
                yield return (i - 1, j);
                if (j > 0)
                {
                    yield return (i - 1, j - 1);
                }
                if (j < _initialState[0].Length - 1)
                {
                    yield return (i - 1, j + 1);
                }
            }
            if (i < _initialState.Length - 1)
            {
                yield return (i + 1, j);
                if (j > 0)
                {
                    yield return (i + 1, j - 1);
                }
                if (j < _initialState[0].Length - 1)
                {
                    yield return (i + 1, j + 1);
                }
            }
        }
    }
}