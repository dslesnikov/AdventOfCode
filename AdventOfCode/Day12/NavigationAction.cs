using System;

namespace AdventOfCode.Day12
{
    public record NavigationAction
    {
        public static NavigationAction Parse(string action)
        {
            var letter = action[0];
            var number = int.Parse(action[1..]);
            return letter switch
            {
                'F' => new MoveAction(MoveDirection.Forward, number),
                'N' => new MoveAction(MoveDirection.North, number),
                'S' => new MoveAction(MoveDirection.South, number),
                'E' => new MoveAction(MoveDirection.East, number),
                'W' => new MoveAction(MoveDirection.West, number),
                'L' => new RotationAction(RotationDirection.Left, number),
                'R' => new RotationAction(RotationDirection.Right, number),
                _ => throw new NotSupportedException()
            };
        }
    }
}