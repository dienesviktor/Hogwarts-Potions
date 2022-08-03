using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;

namespace HogwartsPotions.Models.Implementations;

public class RecipeImplementation : IRecipe
{
    private HogwartsContext _context;

    public RecipeImplementation(HogwartsContext context)
    {
        _context = context;
    }

    public Task<Recipe> GetRecipe(long id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Recipe>> GetAllRecipes()
    {
        throw new System.NotImplementedException();
    }

    public Task AddRecipe(Recipe recipe)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteRecipe(long id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Recipe>> GetAllRecipesWithPotionIngredients(long potionId)
    {
        throw new System.NotImplementedException();
    }

    public Task ChangePotionStatus(Potion potion)
    {
        throw new System.NotImplementedException();
    }
}