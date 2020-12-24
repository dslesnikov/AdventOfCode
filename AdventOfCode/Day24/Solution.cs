using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Day24
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private HexTile[] _tilesToFlip;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 24;

        public async Task InitializeAsync()
        {
            _tilesToFlip = await _reader.ReadAsync(HexTile.Parse);
        }

        public string PartOne()
        {
            var blackTiles = new HashSet<(int X, int Y)>();
            foreach (var tile in _tilesToFlip)
            {
                var coordinates = tile.Coordinates;
                if (!blackTiles.Add(coordinates))
                {
                    blackTiles.Remove(coordinates);
                }
            }
            return blackTiles.Count.ToString();
        }

        public string PartTwo()
        {
            var blackTiles = new HashSet<(int X, int Y)>();
            foreach (var tile in _tilesToFlip)
            {
                var coordinates = tile.Coordinates;
                if (!blackTiles.Add(coordinates))
                {
                    blackTiles.Remove(coordinates);
                }
            }
            Span<(int X, int Y)> neighbours = stackalloc (int, int)[]
            {
                (2, 0),
                (1, 1),
                (-1, 1),
                (-2, 0),
                (-1, -1),
                (1, -1),
                (0, 0)
            };
            for (var i = 0; i < 100; i++)
            {
                var newState = new HashSet<(int X, int Y)>(blackTiles);
                foreach (var tile in blackTiles)
                {
                    foreach (var neighbour in neighbours)
                    {
                        var x = tile.X + neighbour.X;
                        var y = tile.Y + neighbour.Y;
                        var blackAdjacentTiles = 0;
                        foreach (var adjacent in neighbours)
                        {
                            if (adjacent == (0, 0))
                            {
                                continue;
                            }
                            if (blackTiles.Contains((adjacent.X + x, adjacent.Y + y)))
                            {
                                blackAdjacentTiles++;
                            }
                        }
                        if (blackTiles.Contains((x, y)) && (blackAdjacentTiles == 0 || blackAdjacentTiles > 2))
                        {
                            newState.Remove((x, y));
                        }
                        else if (blackAdjacentTiles == 2)
                        {
                            newState.Add((x, y));
                        }
                    }
                }
                blackTiles = newState;
            }
            return blackTiles.Count.ToString();
        }
    }
}