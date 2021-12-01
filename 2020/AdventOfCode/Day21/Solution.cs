using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day21
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private Meal[] _meals;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 21;
        
        public async Task InitializeAsync()
        {
            _meals = await _reader.ReadAsync(Meal.Parse);
        }

        public string PartOne()
        {
            var allergens = GetAllergenList();
            var allergicIngredients = allergens.Values.ToArray();
            var restIngredientsCount = _meals.SelectMany(meal => meal.Ingredients).Count(ingredient => !allergicIngredients.Contains(ingredient));
            return restIngredientsCount.ToString();
        }

        public string PartTwo()
        {
            var allergens = GetAllergenList();
            var result = string.Join(",", allergens.OrderBy(pair => pair.Key).Select(pair => pair.Value));
            return result;
        }

        private Dictionary<string, string> GetAllergenList()
        {
            var allergens = new Dictionary<string, HashSet<string>>();
            foreach (var meal in _meals)
            {
                foreach (var allergen in meal.Allergens)
                {
                    if (allergens.ContainsKey(allergen))
                    {
                        allergens[allergen].IntersectWith(meal.Ingredients);
                    }
                    else
                    {
                        allergens[allergen] = new HashSet<string>(meal.Ingredients);
                    }
                }
            }
            var hasChanges = true;
            var allergensWithOneSuspect = allergens.Where(pair => pair.Value.Count == 1).ToArray();
            while (allergensWithOneSuspect.Length != allergens.Count || hasChanges)
            {
                hasChanges = false;
                foreach (var (allergen, suspectSet) in allergensWithOneSuspect)
                {
                    var otherKeys = allergens.Keys.Where(k => k != allergen);
                    foreach (var key in otherKeys)
                    {
                        var length = allergens[key].Count;
                        allergens[key].ExceptWith(suspectSet);
                        hasChanges = allergens[key].Count != length;
                    }
                }
                allergensWithOneSuspect = allergens.Where(pair => pair.Value.Count == 1).ToArray();
            }
            return allergens.ToDictionary(a => a.Key, a => a.Value.Single());
        }
    }
}