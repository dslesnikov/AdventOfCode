using System.Text;

namespace AdventOfCode._06_WaitForIt;

public class Solution(IReadOnlyList<RaceRecord> records) : ISolution<Solution>
{
    public static int Day => 6;

    public static Solution Parse(string s)
    {
        var split = s.Split('\n');
        var times = split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(int.Parse)
            .ToArray();
        var distances = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(int.Parse)
            .ToArray();
        var records = times.Zip(distances, (time, distance) => new RaceRecord(time, distance))
            .ToArray();
        return new Solution(records);
    }

    public string Part1()
    {
        var waysToWin = records
            .Select(record =>
            {
                var count = 0;
                for (var waitTime = 0; waitTime < record.Time; waitTime++)
                {
                    var distance = waitTime * (record.Time - waitTime);
                    if (distance > record.Distance)
                    {
                        count++;
                    }
                }
                return count;
            })
            .Aggregate(1, (acc, item) => acc * item);
        return waysToWin.ToString();
    }

    public string Part2()
    {
        var timeString = records
            .Aggregate(new StringBuilder(), (sb, record) => sb.Append(record.Time))
            .ToString();
        var time = uint.Parse(timeString);
        var distanceString = records
            .Aggregate(new StringBuilder(), (sb, record) => sb.Append(record.Distance))
            .ToString();
        var distance = ulong.Parse(distanceString);
        var count = 0;
        for (ulong waitTime = 0; waitTime < time; waitTime++)
        {
            var potentialDistance = waitTime * (time - waitTime);
            if (potentialDistance > distance)
            {
                count++;
            }
        }
        return count.ToString();
    }
}