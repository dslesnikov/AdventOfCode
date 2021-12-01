namespace AdventOfCode.Day12
{
    public record RotationAction(RotationDirection Direction, int Angle) : NavigationAction;
}