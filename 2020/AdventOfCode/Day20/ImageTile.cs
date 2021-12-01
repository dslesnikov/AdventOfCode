using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day20
{
    public class ImageTile
    {
        public int Number { get; }
        
        public char[][] Data { get; private set; }

        private ImageTile(int number, char[][] data)
        {
            Number = number;
            Data = data;
        }
        
        public static ImageTile Parse(string raw)
        {
            var lines = raw.Split('\n');
            var number = int.Parse(lines[0][5..^1]);
            var data = lines[1..].Select(line => line.ToCharArray()).ToArray();
            return new ImageTile(number, data);
        }

        public IEnumerable<TileMatch> Match(ImageTile another)
        {
            var match = Data[0].SequenceEqual(another.Data[0]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.Vertically, RotateDirection.None);
            }
            match = Data[0].SequenceEqual(another.Data[0].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.Both, RotateDirection.None);
            }
            match = Data[0].SequenceEqual(another.Data[^1]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.None, RotateDirection.None);
            }
            match = Data[0].SequenceEqual(another.Data[^1].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.Horizontally, RotateDirection.None);
            }
            match = Data[0].SequenceEqual(another.Data.Select(row => row[0]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.None, RotateDirection.Counterclockwise);
            }
            match = Data[0].SequenceEqual(another.Data.Select(row => row[0]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.Vertically, RotateDirection.Clockwise);
            }
            match = Data[0].SequenceEqual(another.Data.Select(row => row[^1]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.Vertically, RotateDirection.Counterclockwise);
            }
            match = Data[0].SequenceEqual(another.Data.Select(row => row[^1]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Up, FlipDirection.None, RotateDirection.Clockwise);
            }
            
            
            match = Data[^1].SequenceEqual(another.Data[0]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.None, RotateDirection.None);
            }
            match = Data[^1].SequenceEqual(another.Data[0].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.Horizontally, RotateDirection.None);
            }
            match = Data[^1].SequenceEqual(another.Data[^1]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.Vertically, RotateDirection.None);
            }
            match = Data[^1].SequenceEqual(another.Data[^1].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.Both, RotateDirection.None);
            }
            match = Data[^1].SequenceEqual(another.Data.Select(row => row[0]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.Horizontally, RotateDirection.Clockwise);
            }
            match = Data[^1].SequenceEqual(another.Data.Select(row => row[0]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.None, RotateDirection.Clockwise);
            }
            match = Data[^1].SequenceEqual(another.Data.Select(row => row[^1]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.None, RotateDirection.Counterclockwise);
            }
            match = Data[^1].SequenceEqual(another.Data.Select(row => row[^1]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Down, FlipDirection.Horizontally, RotateDirection.Counterclockwise);
            }
            
            
            match = Data.Select(row => row[0]).SequenceEqual(another.Data[0]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.None, RotateDirection.Clockwise);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data[0].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.Vertically, RotateDirection.Clockwise);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data[^1]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.Vertically, RotateDirection.Counterclockwise);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data[^1].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.None, RotateDirection.Counterclockwise);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data.Select(row => row[0]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.Horizontally, RotateDirection.None);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data.Select(row => row[0]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.Both, RotateDirection.None);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data.Select(row => row[^1]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.None, RotateDirection.None);
            }
            match = Data.Select(row => row[0]).SequenceEqual(another.Data.Select(row => row[^1]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Left, FlipDirection.Vertically, RotateDirection.None);
            }
            
            
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data[0]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.Vertically, RotateDirection.Counterclockwise);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data[^1]);
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.None, RotateDirection.Clockwise);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data[0].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.None, RotateDirection.Counterclockwise);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data[^1].Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.Vertically, RotateDirection.Clockwise);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data.Select(row => row[0]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.None, RotateDirection.None);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data.Select(row => row[0]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.Vertically, RotateDirection.None);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data.Select(row => row[^1]));
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.Horizontally, RotateDirection.None);
            }
            match = Data.Select(row => row[^1]).SequenceEqual(another.Data.Select(row => row[^1]).Reverse());
            if (match)
            {
                yield return new TileMatch(MatchPosition.Right, FlipDirection.Both, RotateDirection.None);
            }
        } 

        public void Flip(FlipDirection direction)
        {
            switch (direction)
            {
                case FlipDirection.None:
                    return;
                case FlipDirection.Horizontally:
                    MatrixOperations.FlipHorizontally(Data, Data);
                    break;
                case FlipDirection.Vertically:
                    MatrixOperations.FlipVertically(Data, Data);
                    break;
                case FlipDirection.Both:
                    MatrixOperations.FlipHorizontally(Data, Data);
                    MatrixOperations.FlipVertically(Data, Data);
                    break;
            }
        }

        public void Rotate(RotateDirection direction)
        {
            switch (direction)
            {
                case RotateDirection.None:
                    return;
                case RotateDirection.Counterclockwise:
                    MatrixOperations.RotateCounterClockwise(Data, Data);
                    break;
                case RotateDirection.Clockwise:
                    MatrixOperations.RotateClockwise(Data, Data);
                    break;
            }
        }

        public void ShrinkBorder()
        {
            var newData = new char[Data.Length - 2][];
            for (var i = 0; i < newData.Length; i++)
            {
                newData[i] = new char[Data[0].Length - 2];
                for (var j = 0; j < newData[i].Length; j++)
                {
                    newData[i][j] = Data[i + 1][j + 1];
                }
            }
            Data = newData;
        }
    }
}