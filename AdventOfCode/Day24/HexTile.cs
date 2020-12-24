using System;
using System.Collections.Generic;

namespace AdventOfCode.Day24
{
    public record HexTile(int X, int Y)
    {
        public static HexTile Parse(string line)
        {
            var coordinates = (X: 0, Y: 0);
            var index = 0;
            while (index < line.Length)
            {
                coordinates = line[index++] switch
                {
                    'e' => (coordinates.X + 2, coordinates.Y),
                    'w' => (coordinates.X - 2, coordinates.Y),
                    's' => line[index++] switch
                    {
                        'e' => (coordinates.X + 1, coordinates.Y + 1),
                        'w' => (coordinates.X - 1, coordinates.Y + 1),
                        _ => throw new NotSupportedException()
                    },
                    'n' => line[index++] switch
                    {
                        'e' => (coordinates.X + 1, coordinates.Y - 1),
                        'w' => (coordinates.X - 1, coordinates.Y - 1),
                        _ => throw new NotSupportedException()
                    },
                    _ => throw new NotSupportedException()
                };
            }
            return new HexTile(coordinates.X, coordinates.Y);
        }

        public (int X, int Y) Coordinates => (X, Y);
    }
}