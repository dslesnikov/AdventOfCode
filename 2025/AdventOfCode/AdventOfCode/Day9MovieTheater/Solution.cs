namespace AdventOfCode.Day9MovieTheater;

public class Solution : ISolution, IFromLines<Solution, Tile>
{
    public static int Day => 9;
    
    private readonly IReadOnlyList<Tile> _tiles;

    private Solution(IReadOnlyList<Tile> tiles)
    {
        _tiles = tiles;
    }

    public string SolvePartOne()
    {
        var maxArea = 0L;
        for (var i = 0; i < _tiles.Count; i++)
        {
            for (var j = i + 1; j < _tiles.Count; j++)
            {
                var area = _tiles[i].Area(_tiles[j]);
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }
        }
        return maxArea.ToString();
    }

    public string SolvePartTwo()
    {
        var maxArea = 0L;
        var areaTuples = new List<(Tile First, Tile Second, long Area)>();
        for (var i = 0; i < _tiles.Count; i++)
        {
            for (var j = i + 1; j < _tiles.Count; j++)
            {
                areaTuples.Add((_tiles[i], _tiles[j], _tiles[i].Area(_tiles[j])));
            }
        }
        areaTuples.Sort((a, b) => b.Area.CompareTo(a.Area));
        var @lock = new Lock();
        foreach (var page in areaTuples.Chunk(1000))
        {
            Parallel.ForEach(page, item =>
            {
                var inside = true;
                var (first, second, area) = item;
                var minX = Math.Min(first.X, second.X);
                var maxX = Math.Max(first.X, second.X);
                var minY = Math.Min(first.Y, second.Y);
                var maxY = Math.Max(first.Y, second.Y);
                for (var x = minX; x <= maxX; x++)
                {
                    var tile = new Tile(x, minY);
                    if (!IsWithinBounds(tile, _tiles))
                    {
                        inside = false;
                        break;
                    }

                    tile = new Tile(x, maxY);
                    if (!IsWithinBounds(tile, _tiles))
                    {
                        inside = false;
                        break;
                    }
                }

                for (var y = minY; y <= maxY; y++)
                {
                    var tile = new Tile(minX, y);
                    if (!IsWithinBounds(tile, _tiles))
                    {
                        inside = false;
                        break;
                    }

                    tile = new Tile(maxX, y);
                    if (!IsWithinBounds(tile, _tiles))
                    {
                        inside = false;
                        break;
                    }
                }

                if (inside)
                {
                    lock (@lock)
                    {
                        if (area > maxArea)
                        {
                            maxArea = area;
                        }
                    }
                }
            });
            if (maxArea > 0)
            {
                break;
            }
        }
        return maxArea.ToString();
    }

    private bool IsWithinBounds(Tile tile, IReadOnlyList<Tile> polygon)
    {
        static bool PointOnSegment(Tile p, Tile a, Tile b)
        {
            var minX = Math.Min(a.X, b.X);
            var maxX = Math.Max(a.X, b.X);
            var minY = Math.Min(a.Y, b.Y);
            var maxY = Math.Max(a.Y, b.Y);

            // Quick reject by bounding box (also handles degenerate a==b).
            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
                return false;

            // Collinearity via cross product
            long ax = a.X, ay = a.Y;
            long bx = b.X, by = b.Y;
            long px = p.X, py = p.Y;

            var cross = (bx - ax) * (py - ay) - (by - ay) * (px - ax);
            return cross == 0;
        }

        var x = tile.X;
        var y = tile.Y;
        var n = polygon.Count;

        var inside = false;

        for (var i = 0; i < n; i++)
        {
            var a = polygon[i];
            var b = polygon[(i + 1) % n];

            // Treat boundary as inside (covers point-on-edge and point-on-vertex,
            // including collinear “vertex on segment” runs).
            if (PointOnSegment(tile, a, b))
            {
                return true;
            }

            // Ray cast to +X: use a half-open Y test to avoid double-counting at vertices.
            if ((a.Y > y) == (b.Y > y))
                continue;

            // Compare intersection X to point X without floating point:
            // x_int = ax + (bx-ax)*(y-ay)/(by-ay)
            long ax = a.X, ay = a.Y;
            long bx = b.X, by = b.Y;
            long px = x, py = y;

            var dy = by - ay;      // non-zero due to straddle check above
            var dx = bx - ax;

            var left = ax * dy + dx * (py - ay); // numerator expressed over dy
            var right = px * dy;

            // If dy < 0, inequality reverses.
            var intersectsToRight = dy > 0 ? left > right : left < right;

            if (intersectsToRight)
                inside = !inside;
        }

        return inside;
    }


    public static Tile ParseLine(string line)
    {
        return Tile.Parse(line);
    }

    public static Solution FromParsed(IReadOnlyList<Tile> entries)
    {
        return new Solution(entries);
    }
}