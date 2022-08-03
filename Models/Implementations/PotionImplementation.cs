using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;

namespace HogwartsPotions.Models.Implementations;

public class PotionImplementation : IPotion
{
    private HogwartsContext _context;

    public PotionImplementation(HogwartsContext context)
    {
        _context = context;
    }

    public Task<Potion> GetPotion(long potionId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Potion>> GetAllPotions()
    {
        throw new System.NotImplementedException();
    }

    public Task AddPotion(Potion potion)
    {
        throw new System.NotImplementedException();
    }

    public Task DeletePotion(long id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Potion>> GetAllPotionsByStudent(long studentId)
    {
        throw new System.NotImplementedException();
    }

    public Task<Potion> AddEmptyPotion(Student student)
    {
        throw new System.NotImplementedException();
    }

    public Task<Potion> AddIngredientToPotion(long potionId, Ingredient ingred)
    {
        throw new System.NotImplementedException();
    }
}