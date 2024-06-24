namespace AdventOfCode._19_Aplenty;

public class Solution(
    IReadOnlyDictionary<string, Workflow> workflows,
    IReadOnlyList<Part> parts) : ISolution<Solution>
{
    public static int Day => 19;

    public static Solution Parse(string s)
    {
        var split = s.Split("\n\n");
        var workflows = split[0].Split('\n')
            .Select(Workflow.Parse)
            .ToDictionary(x => x.Name);
        var parts = split[1].Split('\n')
            .Select(Part.Parse)
            .ToList();
        return new Solution(workflows, parts);
    }

    public string Part1()
    {
        var accepted = new HashSet<Part>();
        foreach (var part in parts)
        {
            var workflow = workflows["in"];
            var isFinal = false;
            while (!isFinal)
            {
                var result = workflow.Apply(part);
                switch (result)
                {
                    case CheckResult.Accept:
                        accepted.Add(part);
                        isFinal = true;
                        break;
                    case CheckResult.Reject:
                        isFinal = true;
                        break;
                    case CheckResult.Transfer transfer:
                        workflow = workflows[transfer.Name];
                        break;
                }
            }
        }
        return checked(accepted.Sum(p => p.Aerodynamic + p.Musical + p.CoolLooking + p.Shiny))
            .ToString();
    }

    public string Part2()
    {
        var accepted = new HashSet<TraverseState>();
        var queue = new Queue<(TraverseState State, string Workflow, int Rule)>();
        var initialState = new TraverseState(1, 1, 1, 1, 4000, 4000, 4000, 4000);
        queue.Enqueue((initialState, "in", 0));
        while (queue.TryDequeue(out var current))
        {
            var workflow = workflows[current.Workflow];
            var rule = workflow.Rules[current.Rule];
            switch (rule)
            {
                case Rule.Accept:
                    accepted.Add(current.State);
                    break;
                case Rule.Condition condition:
                    var (positive, negative) = current.State.GetSplit(condition);
                    if (positive is not null)
                    {
                        switch (condition.SuccessfulResult)
                        {
                            case CheckResult.Accept:
                                accepted.Add(positive.Value);
                                break;
                            case CheckResult.Transfer transfer:
                                queue.Enqueue((positive.Value, transfer.Name, 0));
                                break;
                        }
                    }
                    if (negative is not null)
                    {
                        queue.Enqueue((negative.Value, current.Workflow, current.Rule + 1));
                    }
                    break;
                case Rule.Transfer transfer:
                    queue.Enqueue((current.State, transfer.Name, 0));
                    break;
            }
        }
        var list = accepted.ToArray();
        checked
        {
            var intersecting = 0L;
            for (var i = 0; i < list.Length; i++)
            {
                for (var j = i + 1; j < list.Length; j++)
                {
                    intersecting = list[i].CalculateIntersection(list[j]);
                }
            }
            var sum = accepted
                .Aggregate(
                    0L,
                    (agg, x) =>
                        agg
                        +
                        (x.MaxCoolLooking - (long)x.MinCoolLooking + 1) *
                        (x.MaxMusical - (long)x.MinMusical + 1) *
                        (x.MaxAerodynamic - (long)x.MinAerodynamic + 1) *
                        (x.MaxShiny - (long)x.MinShiny + 1));
            return (sum - intersecting).ToString();
        }
    }
}