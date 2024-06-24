using Microsoft.Z3;

namespace AdventOfCode._24_NeverTellMeTheOdds;

public class Solution(IReadOnlyList<HailStone> hailStones) : ISolution<Solution>
{
    public static int Day => 24;

    public static Solution Parse(string s)
    {
        var stones = s.Split('\n')
            .Select(HailStone.Parse)
            .ToArray();
        return new Solution(stones);
    }

    public string Part1()
    {
        const decimal min = 200000000000000M;
        const decimal max = 400000000000000M;
        var intersections = 0;
        for (var i = 0; i < hailStones.Count; i++)
        {
            for (var j = i + 1; j < hailStones.Count; j++)
            {
                var first = hailStones[i];
                var second = hailStones[j];
                var intersection = first.GetPlainIntersection(second);
                if (intersection is null ||
                    intersection.Value.X < min ||
                    intersection.Value.X > max ||
                    intersection.Value.Y < min ||
                    intersection.Value.Y > max ||
                    intersection.Value.OurTime < 0 ||
                    intersection.Value.AnotherTime < 0)
                {
                    continue;
                }
                intersections++;
            }
        }
        return intersections.ToString();
    }

    public string Part2()
    {
        var context = new Context();
        var solver = context.MkSolver();
        var stoneX = context.MkIntConst("stoneX");
        var stoneY = context.MkIntConst("stoneY");
        var stoneZ = context.MkIntConst("stoneZ");
        var speedX = context.MkIntConst("speedX");
        var speedY = context.MkIntConst("speedY");
        var speedZ = context.MkIntConst("speedZ");
        for (var i = 0; i < 5; i++)
        {
            var time = context.MkIntConst($"time{i + 1}");
            var hailStone = hailStones[i];
            var hailX = context.MkInt(hailStone.Position.X);
            var hailY = context.MkInt(hailStone.Position.Y);
            var hailZ = context.MkInt(hailStone.Position.Z);
            var hailSpeedX = context.MkInt(hailStone.Velocity.X);
            var hailSpeedY = context.MkInt(hailStone.Velocity.Y);
            var hailSpeedZ = context.MkInt(hailStone.Velocity.Z);
            var leftX = context.MkAdd(stoneX, context.MkMul(time, speedX));
            var leftY = context.MkAdd(stoneY, context.MkMul(time, speedY));
            var leftZ = context.MkAdd(stoneZ, context.MkMul(time, speedZ));
            var rightX = context.MkAdd(hailX, context.MkMul(time, hailSpeedX));
            var rightY = context.MkAdd(hailY, context.MkMul(time, hailSpeedY));
            var rightZ = context.MkAdd(hailZ, context.MkMul(time, hailSpeedZ));
            var xEquality = context.MkEq(leftX, rightX);
            var yEquality = context.MkEq(leftY, rightY);
            var zEquality = context.MkEq(leftZ, rightZ);
            solver.Assert(xEquality);
            solver.Assert(yEquality);
            solver.Assert(zEquality);
            solver.Assert(time > 0);
        }
        var result = solver.Check();
        if (result != Status.SATISFIABLE)
        {
            throw new InvalidOperationException();
        }
        var model = solver.Model;
        var sum = model.Eval(context.MkAdd(stoneX, stoneY, stoneZ));
        var sumValue = (sum as IntNum)!.Int64;
        return sumValue.ToString();
    }
}