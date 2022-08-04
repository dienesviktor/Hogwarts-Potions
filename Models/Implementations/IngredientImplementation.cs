using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Implementations;

public class IngredientImplementation : IIngredient
{
    private HogwartsContext _context;

    public IngredientImplementation(HogwartsContext context)
    {
        _context = context;
    }
    public async Task<Ingredient> GetIngredient(long id)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.ID == id);
    }

    public async Task<List<Ingredient>> GetAllIngredients()
    {
        return await _context.Ingredients
            .ToListAsync();
    }

    public async Task AddIngredient(Ingredient ingredient)
    {
        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteIngredient(long id)
    {
        Ingredient ingredient = GetIngredient(id).Result;
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
    }
}