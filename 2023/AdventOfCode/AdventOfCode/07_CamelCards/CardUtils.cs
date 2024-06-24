namespace AdventOfCode._07_CamelCards;

public static class CardUtils
{
    public static Card Parse(char c)
    {
        return c switch
        {
            '2' => Card.Two,
            '3' => Card.Three,
            '4' => Card.Four,
            '5' => Card.Five,
            '6' => Card.Six,
            '7' => Card.Seven,
            '8' => Card.Eight,
            '9' => Card.Nine,
            'T' => Card.Ten,
            'J' => Card.Jack,
            'Q' => Card.Queen,
            'K' => Card.King,
            'A' => Card.Ace,
            _ => throw new FormatException()
        };
    }

    public static JokerCard Convert(Card card)
    {
        return card switch
        {
            Card.Two => JokerCard.Two,
            Card.Three => JokerCard.Three,
            Card.Four => JokerCard.Four,
            Card.Five => JokerCard.Five,
            Card.Six => JokerCard.Six,
            Card.Seven => JokerCard.Seven,
            Card.Eight => JokerCard.Eight,
            Card.Nine => JokerCard.Nine,
            Card.Ten => JokerCard.Ten,
            Card.Jack => JokerCard.Joker,
            Card.Queen => JokerCard.Queen,
            Card.King => JokerCard.King,
            Card.Ace => JokerCard.Ace,
            _ => throw new ArgumentException("Invalid card value", nameof(card))
        };
    } 
}