using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day12
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private NavigationAction[] _actions;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 12;
        
        public async Task InitializeAsync()
        {
            _actions = await _reader.ReadAsync(NavigationAction.Parse);
        }

        public string PartOne()
        {
            var current = new NavigationState(0, 0, MoveDirection.East);
            current = _actions.Aggregate(current, Move);
            var result = Math.Abs(current.X) + Math.Abs(current.Y);
            return result.ToString();
        }

        public string PartTwo()
        {
            var current = new WaypointNavigationState(0, 0, 10, 1);
            current = _actions.Aggregate(current, Move);
            var result = Math.Abs(current.X) + Math.Abs(current.Y);
            return result.ToString();
        }

        private NavigationState Move(NavigationState current, NavigationAction action)
        {
            switch (action)
            {
                case MoveAction moveAction:
                    var direction = moveAction.Direction == MoveDirection.Forward
                        ? current.Direction
                        : moveAction.Direction;
                    return direction switch
                    {
                        MoveDirection.North => current with { Y = current.Y + moveAction.Distance }, 
                        MoveDirection.South => current with { Y = current.Y - moveAction.Distance },
                        MoveDirection.East => current with { X = current.X + moveAction.Distance },
                        MoveDirection.West => current with { X = current.X - moveAction.Distance },
                        _ => throw new NotSupportedException()
                    };
                case RotationAction rotationAction:
                    var rotationSign = rotationAction.Direction switch
                    {
                        RotationDirection.Left => 1,
                        RotationDirection.Right => -1,
                        _ => throw new NotSupportedException()
                    };
                    var resultDirection = (int) current.Direction + rotationSign * rotationAction.Angle / 90;
                    var remainder = resultDirection % 4;
                    remainder = remainder < 0 ? 4 + remainder : remainder;
                    return current with { Direction = (MoveDirection)remainder };
                default:
                    throw new ArgumentOutOfRangeException(nameof(action));
            }
        }

        private WaypointNavigationState Move(WaypointNavigationState current, NavigationAction action)
        {
            switch (action)
            {
                case MoveAction moveAction:
                    return moveAction.Direction switch
                    {
                        MoveDirection.North => current with { WaypointY = current.WaypointY + moveAction.Distance },
                        MoveDirection.South => current with { WaypointY = current.WaypointY - moveAction.Distance },
                        MoveDirection.East => current with { WaypointX = current.WaypointX + moveAction.Distance },
                        MoveDirection.West => current with { WaypointX = current.WaypointX - moveAction.Distance },
                        MoveDirection.Forward => current with
                        {
                            X = current.X + current.WaypointX * moveAction.Distance,
                            Y = current.Y + current.WaypointY * moveAction.Distance
                        },
                        _ => throw new NotSupportedException()
                    };
                case RotationAction rotationAction:
                    var times = rotationAction.Angle / 90;
                    var waypoint = (current.WaypointX, current.WaypointY);
                    for (var i = 0; i < times; i++)
                    {
                        waypoint = rotationAction.Direction switch
                        {
                            RotationDirection.Left => (-waypoint.WaypointY, waypoint.WaypointX),
                            RotationDirection.Right => (waypoint.WaypointY, -waypoint.WaypointX),
                            _ => throw new NotSupportedException()
                        };
                    }
                    return current with { WaypointX = waypoint.WaypointX, WaypointY = waypoint.WaypointY };
                default:
                    throw new ArgumentOutOfRangeException(nameof(action));
            }
        }
    }
}