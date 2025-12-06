namespace AdventOfCode.Day2GiftShop;

public record IdRange(long First, long Last)
{
    public static IdRange Parse(string text)
    {
        var split = text.Split('-');
        return new IdRange(long.Parse(split[0]), long.Parse(split[1]));
    }
}