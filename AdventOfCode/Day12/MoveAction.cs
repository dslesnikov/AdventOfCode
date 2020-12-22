namespace AdventOfCode.Day12
{
    public record MoveAction(MoveDirection Direction, int Distance) : NavigationAction;
}