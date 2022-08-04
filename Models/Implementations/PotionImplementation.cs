using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Implementations;

public class PotionImplementation : IPotion
{
    private HogwartsContext _context;

    public PotionImplementation(HogwartsContext context)
    {
        _context = context;
    }

    public async Task<Potion> GetPotion(long id)
    {
        return await _context.Potions
            .Include(p => p.Ingredients)
            .FirstOrDefaultAsync(p => p.ID == id);
    }

    public async Task<List<Potion>> GetAllPotions()
    {
        return await _context.Potions
            .ToListAsync();
    }

    public async Task AddPotion(Potion potion)
    {
        if (potion.Ingredients.Any())
        {
            var newPotion = new Potion() { Student = potion.Student, Name = potion.Name };

            foreach (var ingredient in potion.Ingredients)
            {
                if (!_context.Ingredients.Any(i => i.Name == ingredient.Name))
                {
                    await _context.Ingredients.AddAsync(ingredient);
                    newPotion.Ingredients.Add(ingredient);
                }
                else
                {
                    var existingIngredient = await _context.Ingredients.Where(i => i.Name == ingredient.Name).FirstAsync();
                    newPotion.Ingredients.Add(existingIngredient);
                }
            }

            if (newPotion.Ingredients.Count >= 5)
            {
                if (_context.Recipes.AsEnumerable().Any(r => r.Ingredients.SequenceEqual(newPotion.Ingredients)))
                {
                    newPotion.BrewingStatus = BrewingStatus.Replica;
                }
                else
                {
                    newPotion.BrewingStatus = BrewingStatus.Discovery;

                    int studentRecipies = _context.Recipes.Count(r => r.Student.ID == potion.Student.ID) + 1;

                    var discoveredRecipe = new Recipe() { Student = newPotion.Student, Name = $"{newPotion.Student.Name}'s discovery #{studentRecipies}", Ingredients = newPotion.Ingredients };
                    await _context.AddAsync(discoveredRecipe);
                    newPotion.Recipe = discoveredRecipe;

                    if (newPotion.Name is null)
                    {
                        int studentPotions = GetAllPotionsByStudent(newPotion.Student.ID).Result.Count + 1;
                        newPotion.Name = $"{newPotion.Student.Name}'s potion #{studentPotions}";
                    }
                }
            }
            await _context.Potions.AddAsync(newPotion);
            await _context.SaveChangesAsync();
        }
        else
        {
            await _context.Potions.AddAsync(potion);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletePotion(long id)
    {
        Potion potion = GetPotion(id).Result;
        _context.Potions.Remove(potion);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Potion>> GetAllPotionsByStudent(long studentId)
    {
        return await _context.Potions
            .Where(potion => potion.Student.ID == studentId && potion.BrewingStatus == BrewingStatus.Discovery)
            .ToListAsync();
    }

    public async Task<Potion> AddEmptyPotion(Student student)
    {
        Potion potion = new Potion() { Student = student, BrewingStatus = BrewingStatus.Brew };
        await _context.Potions.AddAsync(potion);
        await _context.SaveChangesAsync();
        return potion;
    }

    public async Task<Potion> AddIngredientToPotion(long potionId, Ingredient ingredient)
    {
        Potion potion = await GetPotion(potionId);

        if (potion != null && potion.Ingredients.Count <= 5)
        {
            foreach (Ingredient ingrdnt in potion.Ingredients)
            {
                if (ingredient.Name == ingrdnt.Name)
                {
                    return potion;
                }
            }
            potion.Ingredients.Add(ingredient);

            await _context.SaveChangesAsync();
            return potion;
        }
        return null;
    }
}