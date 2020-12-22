using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day21
{
    public record Meal(string[] Ingredients, string[] Allergens)
    {
        private static readonly Regex MealRegex =
            new("^((?<Ingredient>\\w+) ?)+(\\(contains( (?<Allergen>\\w+),?)+\\))?$", RegexOptions.Compiled);
        
        public static Meal Parse(string line)
        {
            var match = MealRegex.Match(line);
            var ingredients = match.Groups["Ingredient"].Captures.Select(c => c.Value).ToArray();
            var allergens = match.Groups["Allergen"].Captures.Select(c => c.Value).ToArray();
            return new Meal(ingredients, allergens);
        }
    }
}