namespace AdventOfCode._14_ParabolicReflectorDish;

public class Field
{
    private readonly int _ballsCount;
    private readonly TileType[][] _tiles;

    public Field(TileType[][] tiles)
    {
        _tiles = tiles;
        _ballsCount = tiles.Sum(row => row.Count(t => t == TileType.Ball));
    }

    public Field(Field other)
    {
        _tiles = new TileType[other._tiles.Length][];
        for (var row = 0; row < other._tiles.Length; row++)
        {
            _tiles[row] = new TileType[other._tiles[row].Length];
            for (var col = 0; col < other._tiles[row].Length; col++)
            {
                _tiles[row][col] = other._tiles[row][col];
                if (_tiles[row][col] == TileType.Ball)
                {
                    _ballsCount++;
                }
            }
        }
    }

    public int Height => _tiles.Length;

    public FieldState ExtractState()
    {
        var result = new Cell[_ballsCount];
        var index = 0;
        for (var row = 0; row < _tiles.Length; row++)
        {
            for (var col = 0; col < _tiles[row].Length; col++)
            {
                if (_tiles[row][col] == TileType.Ball)
                {
                    result[index++] = new Cell(row, col);
                }
            }
        }
        return new FieldState(result);
    }

    public int GetNorthBeamLoad()
    {
        var result = 0;
        for (var row = 0; row < _tiles.Length; row++)
        {
            for (var col = 0; col < _tiles[row].Length; col++)
            {
                if (_tiles[row][col] == TileType.Ball)
                {
                    result += _tiles.Length - row;
                }
            }
        }
        return result;
    }

    public void TiltNorth()
    {
        for (var col = 0; col < _tiles[0].Length; col++)
        {
            var currentSupport = 0;
            for (var row = 0; row < _tiles.Length; row++)
            {
                switch (_tiles[row][col])
                {
                    case TileType.Solid:
                        currentSupport = row + 1;
                        continue;
                    case TileType.Ball:
                    {
                        var newRow = currentSupport++;
                        _tiles[newRow][col] = TileType.Ball;
                        if (newRow != row)
                        {
                            _tiles[row][col] = TileType.Empty;
                        }
                        break;
                    }
                }
            }
        }
    }

    public void TiltWest()
    {
        for (var row = 0; row < _tiles.Length; row++)
        {
            var currentSupport = 0;
            for (var col = 0; col < _tiles[0].Length; col++)
            {
                switch (_tiles[row][col])
                {
                    case TileType.Solid:
                        currentSupport = col + 1;
                        continue;
                    case TileType.Ball:
                    {
                        var newCol = currentSupport++;
                        _tiles[row][newCol] = TileType.Ball;
                        if (newCol != col)
                        {
                            _tiles[row][col] = TileType.Empty;
                        }
                        break;
                    }
                }
            }
        }
    }

    public void TiltSouth()
    {
        for (var col = 0; col < _tiles[0].Length; col++)
        {
            var currentSupport = _tiles.Length - 1;
            for (var row = _tiles.Length - 1; row >= 0; row--)
            {
                switch (_tiles[row][col])
                {
                    case TileType.Solid:
                        currentSupport = row - 1;
                        continue;
                    case TileType.Ball:
                    {
                        var newRow = currentSupport--;
                        _tiles[newRow][col] = TileType.Ball;
                        if (newRow != row)
                        {
                            _tiles[row][col] = TileType.Empty;
                        }
                        break;
                    }
                }
            }
        }
    }

    public void TiltEast()
    {
        for (var row = 0; row < _tiles.Length; row++)
        {
            var currentSupport = _tiles[0].Length - 1;
            for (var col = _tiles[0].Length - 1; col >= 0; col--)
            {
                switch (_tiles[row][col])
                {
                    case TileType.Solid:
                        currentSupport = col - 1;
                        continue;
                    case TileType.Ball:
                    {
                        var newCol = currentSupport--;
                        _tiles[row][newCol] = TileType.Ball;
                        if (newCol != col)
                        {
                            _tiles[row][col] = TileType.Empty;
                        }
                        break;
                    }
                }
            }
        }
    }
}