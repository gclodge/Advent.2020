using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Advent._2020.Classes
{
    using AllergenMap = Dictionary<string, int>;
    using IngredientMap = Dictionary<string, Dictionary<string, int>>;

    public class Recipe
    {
        private const string _SplitValue = " (contains";

        public List<string> Ingredients { get; } = null;
        public List<string> Allergens { get; } = null;

        public Recipe(string line)
        {
            var segs = line.Split(new string[] { _SplitValue }, StringSplitOptions.RemoveEmptyEntries);
            this.Ingredients = segs.First()
                                   .Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                                   .ToList();
            this.Allergens = segs.Last().Trim().Replace(")", "")
                                 .Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                 .ToList();
        }
    }

    public class AllergenDetector
    {
        public List<Recipe> Recipes { get; } = null;

        public AllergenMap AllergenMap { get; private set; } = null;
        public IngredientMap IngredientMap { get; private set; } = null;

        public Dictionary<string, string> IngredientAllergenMap = new Dictionary<string, string>();

        public AllergenDetector(IEnumerable<string> input)
        {
            //< Parse all the recipes
            this.Recipes = input.Select(x => new Recipe(x)).ToList();
            //< Populate the necessary maps in memory
            PopulateInitialMaps();
            PopulateIngredientAllergenMap();
        }

        private void PopulateInitialMaps()
        {
            //< Instantiate the 'maps'
            this.AllergenMap = new AllergenMap();
            this.IngredientMap = new IngredientMap();
            //< Iterate over each available Recipe
            foreach (var recipe in Recipes)
            {
                //< Iterate over each available allergen
                foreach (var allergen in recipe.Allergens)
                {
                    //< Iterate over each ingredient
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if (!IngredientMap.ContainsKey(ingredient))
                            IngredientMap.Add(ingredient, new AllergenMap());

                        if (!IngredientMap[ingredient].ContainsKey(allergen))
                            IngredientMap[ingredient].Add(allergen, 0);

                        IngredientMap[ingredient][allergen]++;
                    }
                    //< Populate the allergen map (count of times an allergen is encountered across recipes)
                    if (!AllergenMap.ContainsKey(allergen))
                        AllergenMap.Add(allergen, 0);

                    AllergenMap[allergen]++;
                }
            }
        }

        private void PopulateIngredientAllergenMap()
        {
            while (true)
            {
                var singleIngreds = IngredientMap.Where(kvp => kvp.Value.Count(x => x.Value == AllergenMap[x.Key]) == 1);
                if (singleIngreds.Count() == 0)
                    break;

                foreach (var kvp in singleIngreds)
                {
                    string allergen = kvp.Value.First(item => item.Value == AllergenMap[item.Key]).Key;
                    //< Ensure we've got a key ready in the ingredient->allergen map
                    if (!IngredientAllergenMap.ContainsKey(allergen))
                        IngredientAllergenMap.Add(kvp.Key, null);
                    //< Populate the ingredient->allergen map with this match
                    IngredientAllergenMap[kvp.Key] = allergen;
                    //< Set the 'allergen count' to 0 for this allergen for all other ingredients (to remove from consideration)
                    foreach (var ingKvp in IngredientMap)
                    {
                        ingKvp.Value[allergen] = 0;
                    }
                }

            }
        }

        public int CountNonAllergenOccurrence()
        {
            int count = 0;
            foreach (var recipe in Recipes)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!IngredientAllergenMap.ContainsKey(ingredient))
                        count++;
                }
            }
            return count;
        }

        public string GetCanonicalDangerousIngredientList()
        {
            var ordered = IngredientAllergenMap.OrderBy(kvp => kvp.Value).ToList();

            var orderedValues = ordered.Select(kvp => kvp.Key).ToList();

            return string.Join(",", orderedValues);
        }
    }
}
