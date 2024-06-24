namespace AdventOfCode._19_Aplenty;

public readonly record struct TraverseState(
    int MinCoolLooking,
    int MinMusical,
    int MinAerodynamic,
    int MinShiny,
    int MaxCoolLooking,
    int MaxMusical,
    int MaxAerodynamic,
    int MaxShiny)
{
    public (TraverseState? Positive, TraverseState? Negative) GetSplit(Rule.Condition condition)
    {
        var current = condition.Characteristic switch
        {
            Characteristic.CoolLooking => (Min: MinCoolLooking, Max: MaxCoolLooking),
            Characteristic.Musical => (Min: MinMusical, Max: MaxMusical),
            Characteristic.Aerodynamic => (Min: MinAerodynamic, Max: MaxAerodynamic),
            Characteristic.Shiny => (Min: MinShiny, Max: MaxShiny),
            _ => throw new ArgumentOutOfRangeException(nameof(condition))
        };
        switch (condition.Comparison)
        {
            case ComparisonType.Less when current.Min >= condition.Value:
                return (null, this);
            case ComparisonType.Less when current.Max < condition.Value:
                return (this, null);
            case ComparisonType.Greater when current.Max <= condition.Value:
                return (null, this);
            case ComparisonType.Greater when current.Min > condition.Value:
                return (this, null);
        }
        var smaller = condition.Comparison == ComparisonType.Less
            ? condition.Characteristic switch
            {
                Characteristic.Aerodynamic => this with { MaxAerodynamic = condition.Value - 1 },
                Characteristic.CoolLooking => this with { MaxCoolLooking = condition.Value - 1 },
                Characteristic.Musical => this with { MaxMusical = condition.Value - 1 },
                Characteristic.Shiny => this with { MaxShiny = condition.Value - 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(condition))
            }
            : condition.Characteristic switch
            {
                Characteristic.Aerodynamic => this with { MaxAerodynamic = condition.Value },
                Characteristic.CoolLooking => this with { MaxCoolLooking = condition.Value },
                Characteristic.Musical => this with { MaxMusical = condition.Value },
                Characteristic.Shiny => this with { MaxShiny = condition.Value },
                _ => throw new ArgumentOutOfRangeException(nameof(condition))
            };
        var bigger = condition.Comparison == ComparisonType.Greater
            ? condition.Characteristic switch
            {
                Characteristic.Aerodynamic => this with { MinAerodynamic = condition.Value + 1 },
                Characteristic.CoolLooking => this with { MinCoolLooking = condition.Value + 1 },
                Characteristic.Musical => this with { MinMusical = condition.Value + 1 },
                Characteristic.Shiny => this with { MinShiny = condition.Value + 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(condition))
            }
            : condition.Characteristic switch
            {
                Characteristic.Aerodynamic => this with { MinAerodynamic = condition.Value },
                Characteristic.CoolLooking => this with { MinCoolLooking = condition.Value },
                Characteristic.Musical => this with { MinMusical = condition.Value },
                Characteristic.Shiny => this with { MinShiny = condition.Value },
                _ => throw new ArgumentOutOfRangeException(nameof(condition))
            };
        return condition.Comparison switch
        {
            ComparisonType.Less => (smaller, bigger),
            ComparisonType.Greater => (bigger, smaller),
            _ => throw new ArgumentOutOfRangeException(nameof(condition))
        };
    }

    public long CalculateIntersection(TraverseState other)
    {
        if (MinCoolLooking > other.MaxCoolLooking ||
            other.MinCoolLooking > MaxCoolLooking)
        {
            return 0;
        }
        if (MinMusical > other.MaxMusical ||
            other.MinMusical > MaxMusical)
        {
            return 0;
        }
        if (MinAerodynamic > other.MaxAerodynamic ||
            other.MinAerodynamic > MaxAerodynamic)
        {
            return 0;
        }
        if (MinShiny > other.MaxShiny ||
            other.MinShiny > MaxShiny)
        {
            return 0;
        }
        long coolLookingLength =
            Math.Min(MaxCoolLooking, other.MaxCoolLooking) -
            Math.Max(MinCoolLooking, other.MinCoolLooking) + 1;
        long musicalLength =
            Math.Min(MaxMusical, other.MaxMusical) -
            Math.Max(MinMusical, other.MinMusical) + 1;
        long aeroLength =
            Math.Min(MaxAerodynamic, other.MaxAerodynamic) -
            Math.Max(MinAerodynamic, other.MinAerodynamic) + 1;
        long shinyLength =
            Math.Min(MaxShiny, other.MaxShiny) -
            Math.Max(MinShiny, other.MinShiny) + 1;
        return coolLookingLength * musicalLength * aeroLength * shinyLength;
    }
}