namespace AdventOfCode._04_Scratchcards;

public class Solution(IReadOnlyList<Card> cards) : ISolution<Solution>
{
    public static int Day => 4;

    public static Solution Parse(string s)
    {
        var lines = s.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var cards = lines.Select(Card.Parse).ToArray();
        return new Solution(cards);
    }

    public string Part1()
    {
        return cards.Sum(c =>
        {
            var myWinners = c.WinningNumbers.Intersect(c.MyNumbers);
            return (int)Math.Pow(2, myWinners.Count - 1);
        }).ToString();
    }

    public string Part2()
    {
        var count = new int[cards.Count];
        Array.Fill(count, 1);
        for (var i = 0; i < cards.Count; i++)
        {
            var winners = cards[i].MyNumbers.Intersect(cards[i].WinningNumbers).Count;
            for (var j = 1; j <= winners && i + j < cards.Count; j++)
            {
                count[i + j] += count[i];
            }
        }
        return count.Sum().ToString();
    }
}