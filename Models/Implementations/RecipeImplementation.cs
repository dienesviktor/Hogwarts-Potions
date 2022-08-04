using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Implementations;

public class RecipeImplementation : IRecipe
{
    private HogwartsContext _context;

    public RecipeImplementation(HogwartsContext context)
    {
        _context = context;
    }

    public async Task<Recipe> GetRecipe(long id)
    {
        return await _context.Recipes
            .FirstOrDefaultAsync(r => r.ID == id);
    }

    public async Task<List<Recipe>> GetAllRecipes()
    {
        return await _context.Recipes
            .Include(r => r.Student)
            .Include(r => r.Ingredients)
            .ToListAsync();
    }

    public async Task AddRecipe(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRecipe(long id)
    {
        Recipe recipe = GetRecipe(id).Result;
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Recipe>> GetAllRecipesWithPotionIngredients(long potionId)
    {
        Potion potion = await _context.Potions
            .Include(p => p.Ingredients)
            .FirstAsync(p => p.ID == potionId);

        return await _context.Recipes
            .Include(r => r.Student)
            .Include(r => r.Ingredients)
            .Where(r => r.Ingredients.Any(i => potion.Ingredients.Contains(i)))
            .ToListAsync();
    }

    public async Task ChangePotionStatus(Potion potion)
    {
        var recipes = await GetAllRecipes();

        foreach (var recipe in recipes)
        {
            if (recipe.Ingredients.SequenceEqual(potion.Ingredients))
            {
                potion.BrewingStatus = BrewingStatus.Replica;
                potion.Recipe = recipe;
                await _context.SaveChangesAsync();
            }
        }
        potion.BrewingStatus = BrewingStatus.Discovery;

        int studentRecipies = _context.Recipes.Count(r => r.Student.ID == potion.Student.ID) + 1;

        var newRecipe = new Recipe() { Student = potion.Student, Ingredients = potion.Ingredients, Name = $"{potion.Student.Name}'s discovery #{studentRecipies}" };
        await AddRecipe(newRecipe);
        potion.Recipe = newRecipe;
        await _context.SaveChangesAsync();
    }
}