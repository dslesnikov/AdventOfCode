using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode._12_HotSprings;

public record ConditionRecord(ImmutableArray<ConditionType> Conditions, ImmutableArray<int> Groups)
    : ISimpleParsable<ConditionRecord>
{
    public static ConditionRecord Parse(string s)
    {
        var split = s.Split(' ');
        var conditions = split[0]
            .Select(c => c switch
            {
                '.' => ConditionType.Operational,
                '?' => ConditionType.Unknown,
                '#' => ConditionType.Damaged,
                _ => throw new FormatException()
            })
            .ToImmutableArray();
        var groups = split[1].Split(',').Select(int.Parse).ToImmutableArray();
        return new ConditionRecord(conditions, groups);
    }

    public ConditionRecord Unfold()
    {
        var newConditions = Enumerable.Repeat(Conditions.Append(ConditionType.Unknown), 4)
            .SelectMany(x => x)
            .Concat(Conditions)
            .ToImmutableArray();
        var newGroups = Enumerable.Repeat(Groups, 5)
            .SelectMany(x => x)
            .ToImmutableArray();
        return new ConditionRecord(newConditions, newGroups);
    }

    public long CountValidArrangements()
    {
        var counted = new Dictionary<(int, int, int), long>();
        return CountValidArrangements(0, 0, 0, counted);
    }
    
    private long CountValidArrangements(
        int index, int groupIndex, int currentGroupLength,
        Dictionary<(int, int, int), long> counted)
    {
        if (index == Conditions.Length &&
            groupIndex == Groups.Length)
        {
            return 1L;
        }
        if (index == Conditions.Length &&
            groupIndex == Groups.Length - 1 &&
            currentGroupLength == Groups[groupIndex])
        {
            return 1L;
        }
        if (index == Conditions.Length)
        {
            return 0L;
        }
        if (counted.TryGetValue((index, groupIndex, currentGroupLength), out var cached))
        {
            return cached;
        }
        var currentCondition = Conditions[index];
        switch (currentCondition)
        {
            case ConditionType.Damaged when groupIndex == Groups.Length ||
                                            Groups[groupIndex] == currentGroupLength:
                return 0L;
            case ConditionType.Damaged:
            {
                var result = CountValidArrangements(index + 1, groupIndex, currentGroupLength + 1, counted);
                counted.Add((index, groupIndex, currentGroupLength), result);
                return result;
            }
            case ConditionType.Unknown:
            {
                var continueGroup = 0L;
                if (groupIndex < Groups.Length &&
                    currentGroupLength < Groups[groupIndex])
                {
                    continueGroup = CountValidArrangements(index + 1, groupIndex, currentGroupLength + 1, counted);
                }
                var finishGroup = 0L;
                if (groupIndex < Groups.Length &&
                    currentGroupLength == Groups[groupIndex])
                {
                    finishGroup = CountValidArrangements(index + 1, groupIndex + 1, 0, counted);
                }
                var doNotStartNewGroup = 0L;
                if (currentGroupLength == 0)
                {
                    doNotStartNewGroup = CountValidArrangements(index + 1, groupIndex, 0, counted);
                }
                var result = continueGroup + finishGroup + doNotStartNewGroup;
                counted.Add((index, groupIndex, currentGroupLength), result);
                return result;
            }
            case ConditionType.Operational when currentGroupLength != 0 &&
                                                Groups[groupIndex] != currentGroupLength:
                return 0L;
            case ConditionType.Operational:
            {
                var newGroupIndex = currentGroupLength == 0 ? groupIndex : groupIndex + 1;
                var result = CountValidArrangements(index + 1, newGroupIndex, 0, counted);
                counted.Add((index, groupIndex, currentGroupLength), result);
                return result;
            }
            default:
                throw new UnreachableException();
        }
    }
}