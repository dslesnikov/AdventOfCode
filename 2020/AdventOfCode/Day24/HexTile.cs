using System;

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
                (int X, int Y) shift = line[index++] switch
                {
                    'e' => (2, 0),
                    'w' => (-2, 0),
                    's' => line[index++] switch
                    {
                        'e' => (1, 1),
                        'w' => (-1, 1),
                        _ => throw new NotSupportedException()
                    },
                    'n' => line[index++] switch
                    {
                        'e' => (1, -1),
                        'w' => (-1, -1),
                        _ => throw new NotSupportedException()
                    },
                    _ => throw new NotSupportedException()
                };
                coordinates = (coordinates.X + shift.X, coordinates.Y + shift.Y);
            }
            return new HexTile(coordinates.X, coordinates.Y);
        }

        public (int X, int Y) Coordinates => (X, Y);
    }
}