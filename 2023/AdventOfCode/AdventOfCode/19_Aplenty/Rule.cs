using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AdventOfCode._19_Aplenty;

public abstract partial class Rule(Func<Part, CheckResult> check) : ISimpleParsable<Rule>
{
    [GeneratedRegex("^\\w+$")]
    private static partial Regex Final();
    [GeneratedRegex(@"^(?<Characteristic>\w)(?<Condition>[<>])(?<Value>\d+):(?<Result>\w+)$")]
    private static partial Regex Conditional();

    public static Rule Parse(string s)
    {
        var match = Final().Match(s);
        if (match.Success)
        {
            return match.Value switch
            {
                "A" => new Accept(),
                "R" => new Reject(),
                _ => new Transfer(match.Value)
            };
        }
        match = Conditional().Match(s);
        if (!match.Success)
        {
            throw new FormatException();
        }
        var characteristic = match.Groups["Characteristic"].Value;
        var parameter = Expression.Parameter(typeof(Part), "part");
        var propertyInfo = characteristic switch
        {
            "x" => typeof(Part).GetProperty(nameof(Part.CoolLooking)),
            "m" => typeof(Part).GetProperty(nameof(Part.Musical)),
            "a" => typeof(Part).GetProperty(nameof(Part.Aerodynamic)),
            "s" => typeof(Part).GetProperty(nameof(Part.Shiny)),
            _ => throw new FormatException()
        };
        var access = Expression.Property(parameter, propertyInfo!);
        var condition = match.Groups["Condition"].Value;
        var value = int.Parse(match.Groups["Value"].Value);
        var comparison = condition switch
        {
            "<" => Expression.LessThan(access, Expression.Constant(value)),
            ">" => Expression.GreaterThan(access, Expression.Constant(value)),
            _ => throw new FormatException()
        };
        var result = match.Groups["Result"].Value;
        CheckResult successExpression = result switch
        {
            "A" => CheckResult.Accept.Instance,
            "R" => CheckResult.Reject.Instance,
            _ => new CheckResult.Transfer(result)
        };
        CheckResult failureExpression = CheckResult.Failed.Instance;
        var ifThenElse = Expression.Condition(comparison,
            Expression.Constant(successExpression),
            Expression.Constant(failureExpression),
            typeof(CheckResult));
        var expression = Expression.Lambda<Func<Part, CheckResult>>(ifThenElse, parameter);
        var check = expression.Compile();
        return new Condition(check)
        {
            Characteristic = characteristic switch
            {
                "x" => Characteristic.CoolLooking,
                "m" => Characteristic.Musical,
                "a" => Characteristic.Aerodynamic,
                "s" => Characteristic.Shiny,
                _ => throw new FormatException()
            },
            Value = value,
            Comparison = condition switch
            {
                "<" => ComparisonType.Less,
                ">" => ComparisonType.Greater,
                _ => throw new FormatException()
            },
            SuccessfulResult = successExpression
        };
    }

    public CheckResult Check(Part part) => check(part);

    public class Accept() : Rule(_ => CheckResult.Accept.Instance);
    public class Reject() : Rule(_ => CheckResult.Reject.Instance);

    public class Transfer(string name) : Rule(_ => new CheckResult.Transfer(name))
    {
        public string Name { get; } = name;
    }

    public class Condition(Func<Part, CheckResult> check) : Rule(check)
    {
        public required Characteristic Characteristic { get; init; }

        public required ComparisonType Comparison { get; init; }

        public required int Value { get; init; }

        public required CheckResult SuccessfulResult { get; init; }
    }
}