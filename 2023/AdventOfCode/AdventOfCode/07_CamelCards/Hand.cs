namespace AdventOfCode._07_CamelCards;

public record Hand<TCard>(IReadOnlyList<TCard> Cards, int Bet) : ISimpleParsable<Hand<Card>>, IComparable<Hand<TCard>>
    where TCard : unmanaged, Enum
{
    private HandType? _type;

    public static Hand<Card> Parse(string s)
    {
        var split = s.Split(' ');
        var cards = split[0].Select(CardUtils.Parse).ToList();
        var bet = int.Parse(split[1]);
        return new Hand<Card>(cards, bet);
    }

    public HandType Type
    {
        get
        {
            _type ??= CalculateType();
            return _type.Value;
        }
    }

    public int CompareTo(Hand<TCard>? other)
    {
        if (other is null)
        {
            return -1;
        }
        if (Type != other.Type)
        {
            return Type.CompareTo(other.Type);
        }
        foreach (var (first, second) in Cards.Zip(other.Cards))
        {
            var comparison = first.CompareTo(second); 
            if (comparison != 0)
            {
                return comparison;
            }
        }
        return 0;
    }

    private HandType CalculateType()
    {
        if (this is Hand<JokerCard> jokerHand && jokerHand.Cards.Contains(JokerCard.Joker))
        {
            return CalculateJokerHandType(jokerHand);
        }
        return CalculateSimpleHandType(Cards);
    }

    private static HandType CalculateJokerHandType(Hand<JokerCard> hand)
    {
        var cards = new Dictionary<JokerCard, (int Count, int FirstIndex)>();
        for (var i = 0; i < hand.Cards.Count; i++)
        {
            var card = hand.Cards[i];
            if (card == JokerCard.Joker)
            {
                continue;
            }
            if (cards.TryGetValue(card, out var stats))
            {
                cards[card] = (stats.Count + 1, stats.FirstIndex);
                continue;
            }
            cards[card] = (1, i);
        }
        var ordered = cards
            .OrderByDescending(x => x.Value.Count)
            .ThenByDescending(x => x.Value.FirstIndex)
            .ToArray();
        var replacement =
            ordered.Length == 0
                ? JokerCard.Ace
                : ordered.Length == 1
                    ? ordered[0].Key
                    : ordered[0].Value.Count > ordered[1].Value.Count
                        ? ordered[0].Key
                        : ordered[1].Key;
        var newHand = hand.Cards
            .Select(x => x == JokerCard.Joker ? replacement : x)
            .ToArray();
        var newHandType = CalculateSimpleHandType(newHand);
        return newHandType;
    }

    private static HandType CalculateSimpleHandType<T>(IReadOnlyList<T> cards)
    {
        var count = cards
            .GroupBy(x => x)
            .Select(x => (Card: x.Key, Count: x.Count()))
            .ToList();
        return count.Count switch
        {
            1 => HandType.FiveOfAKind,
            2 => count.Any(x => x.Count == 4) ? HandType.FourOfAKind : HandType.FullHouse,
            3 => count.Any(x => x.Count == 3) ? HandType.ThreeOfAKind : HandType.TwoPairs,
            _ => count.Count == 4 ? HandType.OnePair : HandType.HighCard
        };
    }
}