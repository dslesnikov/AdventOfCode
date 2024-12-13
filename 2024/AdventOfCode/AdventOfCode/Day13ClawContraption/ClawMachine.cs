namespace AdventOfCode.Day13ClawContraption;

public record ClawMachine(Button A, Button B, Point Prize)
{
    public static ClawMachine FromString(ReadOnlySpan<char> line)
    {
        var split = line.Split('\n');
        split.MoveNext();
        var buttonA = Button.FromString(line[split.Current.Start..split.Current.End]);
        split.MoveNext();
        var buttonB = Button.FromString(line[split.Current.Start..split.Current.End]);
        split.MoveNext();
        var prize = Point.FromString(line[split.Current.Start..split.Current.End]);
        return new ClawMachine(buttonA, buttonB, prize);
    }

    public long Solve(long limit)
    {
        var xgcd = Gcd(A.Move.X, B.Move.X);
        if (Prize.X % xgcd != 0)
        {
            return 0;
        }
        var ygcd = Gcd(A.Move.Y, B.Move.Y);
        if (Prize.Y % ygcd != 0)
        {
            return 0;
        }
        var a1 = A.Move.X;
        var b1 = B.Move.X;
        var c1 = Prize.X;
        var a2 = A.Move.Y;
        var b2 = B.Move.Y;
        var c2 = Prize.Y;
        var y = (c1 * (double)a2 - c2 * a1) / (b1 * (double)a2 - b2 * a1);
        var x = (c2 - b2 * y) / a2;
        if (x > limit || x < 0 || y > limit || y < 0)
        {
            return 0;
        }
        if (!double.IsInteger(x) || !double.IsInteger(y))
        {
            return 0;
        }
        return (long)x * 3 + (long)y;
    }

    private static long Gcd(long a, long b)
    {
        (a, b) = a > b ? (a, b) : (b, a);
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }
        return a;
    }
}