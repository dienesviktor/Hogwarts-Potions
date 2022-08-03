using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IRecipe
{
    public Task<Recipe> GetRecipe(long id);
    public Task<List<Recipe>> GetAllRecipes();
    public Task AddRecipe(Recipe recipe);
    public Task DeleteRecipe(long id);
    public Task<List<Recipe>> GetAllRecipesWithPotionIngredients(long potionId);
    public Task ChangePotionStatus(Potion potion);
}