using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IPotion
{
    public Task<Potion> GetPotion(long potionId);
    public Task<List<Potion>> GetAllPotions();
    public Task AddPotion(Potion potion);
    public Task DeletePotion(long id);
    public Task<List<Potion>> GetAllPotionsByStudent(long studentId);
    public Task<Potion> AddEmptyPotion(Student student);
    public Task<Potion> AddIngredientToPotion(long potionId, Ingredient ingred);
}