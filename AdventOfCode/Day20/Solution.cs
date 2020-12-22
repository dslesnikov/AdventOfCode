using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day20
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private ImageTile[] _tiles;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 20;

        public async Task InitializeAsync()
        {
            var input = await _reader.ReadTextAsync();
            var tiles = input.Split("\n\n");
            _tiles = tiles.Select(ImageTile.Parse).ToArray();
        }

        public string PartOne()
        {
            var imageMap = ConstructImage(_tiles);
            var first = imageMap[0, 0].Number;
            var second = imageMap[0, ^1].Number;
            var third = imageMap[^1, 0].Number;
            var fourth = imageMap[^1, ^1].Number;
            var result = (ulong) first * (ulong) second * (ulong) third * (ulong) fourth;
            return result.ToString();
        }

        public string PartTwo()
        {
            var map = ConstructImage(_tiles);
            foreach (var tile in _tiles)
            {
                tile.ShrinkBorder();
            }
            var flat = map.ExtractImage();
            var variations = new char[8][][];
            variations[0] = flat;
            for (var i = 1; i < variations.Length; i++)
            {
                variations[i] = new char[flat.Length][];
                for (var j = 0; j < variations[i].Length; j++)
                {
                    variations[i][j] = new char[flat[0].Length];
                }
            }
            MatrixOperations.RotateClockwise(variations[0], variations[1]);
            MatrixOperations.RotateClockwise(variations[1], variations[2]);
            MatrixOperations.RotateCounterClockwise(variations[0], variations[3]);
            MatrixOperations.FlipVertically(variations[0], variations[4]);
            MatrixOperations.RotateClockwise(variations[4], variations[5]);
            MatrixOperations.RotateClockwise(variations[4], variations[6]);
            MatrixOperations.RotateCounterClockwise(variations[4], variations[7]);
            var correctVariation = variations
                .Select(variation =>
                {
                    var monsters = 0;
                    for (var i = 0; i < variation.Length; i++)
                    {
                        for (var j = 0; j < variation[i].Length; j++)
                        {
                            if (CheckForSeaMonster(variation, i, j))
                            {
                                monsters++;
                            }
                        }
                    }
                    return (Image: variation, Count: monsters);
                })
                .Single(pair => pair.Count > 0);
            var result = correctVariation.Image.Sum(row => row.Count(c => c == '#')) - correctVariation.Count * 15;
            return result.ToString();
        }

        private bool CheckForSeaMonster(char[][] image, int y, int x)
        {
            const int seaMonsterHeight = 3;
            const int seaMonsterWidth = 20;
            if (y + seaMonsterHeight > image.Length ||
                x + seaMonsterWidth > image[0].Length)
            {
                return false;
            }
            return image[y][x + 18] == '#' &&
                   image[y + 1][x] == '#' &&
                   image[y + 1][x + 5] == '#' &&
                   image[y + 1][x + 6] == '#' &&
                   image[y + 1][x + 11] == '#' &&
                   image[y + 1][x + 12] == '#' &&
                   image[y + 1][x + 17] == '#' &&
                   image[y + 1][x + 18] == '#' &&
                   image[y + 1][x + 19] == '#' &&
                   image[y + 2][x + 1] == '#' &&
                   image[y + 2][x + 4] == '#' &&
                   image[y + 2][x + 7] == '#' &&
                   image[y + 2][x + 10] == '#' &&
                   image[y + 2][x + 13] == '#' &&
                   image[y + 2][x + 16] == '#';
        }

        private static ImageMap ConstructImage(ImageTile[] tiles)
        {
            var map = new ImageMap();
            var unplaced = new HashSet<ImageTile>(tiles);
            while (map.Count != tiles.Length)
            {
                var placed = new List<ImageTile>();
                foreach (var tile in unplaced)
                {
                    if (map.TryAdd(tile))
                    {
                        placed.Add(tile);
                    }
                }
                placed.ForEach(tile => unplaced.Remove(tile));
            }
            return map;
        }
    }
}