namespace AdventOfCode._07_CamelCards;

public class Solution(IReadOnlyList<Hand<Card>> hands) : ISolution<Solution>
{
    public static int Day => 7;

    public static Solution Parse(string s)
    {
        var split = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var hands = split.Select(Hand<Card>.Parse).ToList();
        return new Solution(hands);
    }

    public string Part1()
    {
        var sorted = hands.Order().ToArray();
        var total = sorted.Select((t, i) => (i + 1) * t.Bet).Sum();
        return total.ToString();
    }

    public string Part2()
    {
        var converted = hands
            .Select(x => new Hand<JokerCard>(x.Cards.Select(CardUtils.Convert).ToArray(), x.Bet))
            .Order()
            .ToArray();
        var total = converted.Select((t, i) => (i + 1) * t.Bet).Sum();
        return total.ToString();
    }
}