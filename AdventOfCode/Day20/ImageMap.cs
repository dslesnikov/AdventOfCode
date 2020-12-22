using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day20
{
    public class ImageMap
    {
        private readonly Dictionary<(int X, int Y), ImageTile> _map = new();

        private int _minX = int.MaxValue;
        private int _maxX = int.MinValue;
        private int _minY = int.MaxValue;
        private int _maxY = int.MinValue;

        public int Width => _maxX - _minX + 1;

        public int Height => _maxY - _minY + 1;

        public int Count => _map.Count;

        public ImageTile this[Index x, Index y]
        {
            get
            {
                var offsetX = x.GetOffset(Width);
                var offsetY = y.GetOffset(Height);
                return _map[(offsetX + _minX, offsetY + _minY)];
            }
        }

        public char[][] ExtractImage()
        {
            var sample = _map.First().Value.Data;
            var tileHeight = sample.Length;
            var tileWidth = sample[0].Length;
            var result = new char[Height * tileHeight][];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new char[Width * tileWidth];
            }
            for (var x = _minX; x <= _maxX; x++)
            {
                for (var y = _minY; y <= _maxY; y++)
                {
                    var tile = _map[(x, y)];
                    for (var i = 0; i < tileHeight; i++)
                    {
                        for (var j = 0; j < tileWidth; j++)
                        {
                            result[(y - _minY) * tileHeight + i][(x - _minX) * tileWidth + j] = tile.Data[i][j];
                        }
                    }
                }
            }
            return result;
        }

        public bool TryAdd(ImageTile tileToPlace)
        {
            if (_map.Count == 0)
            {
                _map[(0, 0)] = tileToPlace;
                _maxX = _minX = _maxY = _minY = 0;
                return true;
            }
            Span<(int X, int Y)> freeTiles = stackalloc (int, int)[4];
            foreach (var (x, y) in _map.Keys)
            {
                var freeTilesCount = GetFreeNearestTiles(x, y, freeTiles);
                for (var i = 0; i < freeTilesCount; i++)
                {
                    var (freeX, freeY) = freeTiles[i];
                    var restrictions = GetNeighbourRestrictions(freeX, freeY);
                    TileMatch match = default;
                    if (restrictions.All(restriction => restriction.TryMatch(tileToPlace, out match)))
                    {
                        tileToPlace.Rotate(match.Rotate);
                        tileToPlace.Flip(match.Flip);
                        _map.Add((freeX, freeY), tileToPlace);
                        _maxX = Math.Max(freeX, _maxX);
                        _minX = Math.Min(freeX, _minX);
                        _maxY = Math.Max(freeY, _maxY);
                        _minY = Math.Min(freeY, _minY);
                        return true;
                    }
                }
            }
            return false;
        }

        private int GetFreeNearestTiles(int x, int y, Span<(int X,int Y)> tiles)
        {
            var index = 0;
            if (!_map.ContainsKey((x - 1, y)))
            {
                tiles[index++] = (x - 1, y);
            }
            if (!_map.ContainsKey((x + 1, y)))
            {
                tiles[index++] = (x + 1, y);
            }
            if (!_map.ContainsKey((x, y - 1)))
            {
                tiles[index++] = (x, y - 1);
            }
            if (!_map.ContainsKey((x, y + 1)))
            {
                tiles[index++] = (x, y + 1);
            }
            return index;
        }

        private IEnumerable<NeighbourRestriction> GetNeighbourRestrictions(int x, int y)
        {
            if (_map.ContainsKey((x - 1, y)))
            {
                yield return new NeighbourRestriction
                {
                    Tile = _map[(x - 1, y)],
                    ExpectedPosition = MatchPosition.Right
                };
            }
            if (_map.ContainsKey((x + 1, y)))
            {
                yield return new NeighbourRestriction
                {
                    Tile = _map[(x + 1, y)],
                    ExpectedPosition = MatchPosition.Left
                };
            }
            if (_map.ContainsKey((x, y - 1)))
            {
                yield return new NeighbourRestriction
                {
                    Tile = _map[(x, y - 1)],
                    ExpectedPosition = MatchPosition.Down
                };
            }
            if (_map.ContainsKey((x, y + 1)))
            {
                yield return new NeighbourRestriction
                {
                    Tile = _map[(x, y + 1)],
                    ExpectedPosition = MatchPosition.Up
                };
            }
        }

        private readonly struct NeighbourRestriction
        {
            public ImageTile Tile { get; init; }
            
            public MatchPosition ExpectedPosition { get; init; }

            public bool TryMatch(ImageTile tile, out TileMatch match)
            {
                foreach (var potentialMatch in Tile.Match(tile))
                {
                    if (potentialMatch.Position == ExpectedPosition)
                    {
                        match = potentialMatch;
                        return true;
                    }
                }
                match = default;
                return false;
            }
        }
    }
}