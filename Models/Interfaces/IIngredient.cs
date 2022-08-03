using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IIngredient
{
    public Task<Ingredient> GetIngredient(long id);
    public Task<List<Ingredient>> GetAllIngredients();
    public Task AddIngredient(Ingredient ingredient);
    public Task DeleteIngredient(long id);
}