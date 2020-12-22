using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day17
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private HashSet<(int X, int Y, int Z)> _threeDimensionalActiveCells;
        private HashSet<(int X, int Y, int Z, int W)> _fourDimensionalActiveCells;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 17;
        
        public async Task InitializeAsync()
        {
            var lines = await _reader.ReadLinesAsync();
            _threeDimensionalActiveCells = new HashSet<(int X, int Y, int Z)>();
            _fourDimensionalActiveCells = new HashSet<(int X, int Y, int Z, int W)>();
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        _threeDimensionalActiveCells.Add((x, y, 0));
                        _fourDimensionalActiveCells.Add((x, y, 0, 0));
                    }
                }
            }
        }

        public string PartOne()
        {
            var nearestCells = new[] {-1, 0, 1};
            var neighbourhood = nearestCells
                .SelectMany(x => nearestCells.SelectMany(y => nearestCells.Select<int, (int X,int Y,int Z)>(z => (x, y, z))))
                .ToArray();
            var neighbours = neighbourhood.Where(n => n != default).ToArray();
            for (var i = 0; i < 6; i++)
            {
                var newState = new HashSet<(int X, int Y, int Z)>(_threeDimensionalActiveCells);
                foreach (var activeCell in _threeDimensionalActiveCells)
                {
                    foreach (var neighbourhoodCell in neighbourhood)
                    {
                        var current = (
                            X: activeCell.X + neighbourhoodCell.X,
                            Y: activeCell.Y + neighbourhoodCell.Y,
                            Z: activeCell.Z + neighbourhoodCell.Z);
                        var activeCellCount = 0;
                        foreach (var neighbour in neighbours)
                        {
                            var cell = (
                                X: current.X + neighbour.X,
                                Y: current.Y + neighbour.Y,
                                Z: current.Z + neighbour.Z);
                            if (_threeDimensionalActiveCells.Contains(cell))
                            {
                                activeCellCount++;
                            }
                        }
                        if (_threeDimensionalActiveCells.Contains(current) && activeCellCount != 2 && activeCellCount != 3)
                        {
                            newState.Remove(current);
                        }
                        if (!_threeDimensionalActiveCells.Contains(current) && activeCellCount == 3)
                        {
                            newState.Add(current);
                        }
                    }
                }
                _threeDimensionalActiveCells = newState;
            }
            return _threeDimensionalActiveCells.Count.ToString();
        }

        public string PartTwo()
        {
            var nearestCells = new[] {-1, 0, 1};
            var neighbourhood = nearestCells
                .SelectMany(x => nearestCells
                    .SelectMany(y => nearestCells
                        .SelectMany(z => nearestCells
                            .Select<int, (int X, int Y, int Z, int W)>(w => (x, y, z, w)))))
                .ToArray();
            var neighbours = neighbourhood.Where(n => n != default).ToArray();
            for (var i = 0; i < 6; i++)
            {
                var newState = new HashSet<(int X, int Y, int Z, int W)>(_fourDimensionalActiveCells);
                foreach (var activeCell in _fourDimensionalActiveCells)
                {
                    foreach (var neighbourhoodCell in neighbourhood)
                    {
                        var current = (
                            X: activeCell.X + neighbourhoodCell.X,
                            Y: activeCell.Y + neighbourhoodCell.Y,
                            Z: activeCell.Z + neighbourhoodCell.Z,
                            W: activeCell.W + neighbourhoodCell.W);
                        var activeCellCount = 0;
                        foreach (var neighbour in neighbours)
                        {
                            var cell = (
                                X: current.X + neighbour.X,
                                Y: current.Y + neighbour.Y,
                                Z: current.Z + neighbour.Z,
                                W: current.W + neighbour.W);
                            if (_fourDimensionalActiveCells.Contains(cell))
                            {
                                activeCellCount++;
                            }
                        }
                        if (_fourDimensionalActiveCells.Contains(current) && activeCellCount != 2 && activeCellCount != 3)
                        {
                            newState.Remove(current);
                        }
                        if (!_fourDimensionalActiveCells.Contains(current) && activeCellCount == 3)
                        {
                            newState.Add(current);
                        }
                    }
                }
                _fourDimensionalActiveCells = newState;
            }
            return _fourDimensionalActiveCells.Count.ToString();
        }
    }
}