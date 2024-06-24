using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode._19_Aplenty;

public partial record Workflow(string Name, IReadOnlyList<Rule> Rules) : ISimpleParsable<Workflow>
{
    [GeneratedRegex(@"(?<Name>\w+){(?<Rules>.*)}")]
    private static partial Regex Pattern();

    public static Workflow Parse(string s)
    {
        var match = Pattern().Match(s);
        if (!match.Success)
        {
            throw new FormatException();
        }
        var name = match.Groups["Name"].Value;
        var rules = match.Groups["Rules"].Value.Split(',').Select(Rule.Parse).ToList();
        return new Workflow(name, rules);
    }

    public CheckResult Apply(Part part)
    {
        foreach (var rule in Rules)
        {
            var result = rule.Check(part);
            switch (result)
            {
                case CheckResult.Accept:
                    return result;
                case CheckResult.Reject:
                    return result;
                case CheckResult.Transfer transfer:
                    return transfer;
            }
        }
        throw new UnreachableException();
    }
}