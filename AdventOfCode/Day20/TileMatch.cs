namespace AdventOfCode.Day20
{
    public readonly struct TileMatch
    {
        public MatchPosition Position { get; init; }
        
        public FlipDirection Flip { get; init; }
        
        public RotateDirection Rotate { get; init; }

        public TileMatch(MatchPosition position, FlipDirection flip, RotateDirection rotate)
        {
            Position = position;
            Flip = flip;
            Rotate = rotate;
        }
    }
}