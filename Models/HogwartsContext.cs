using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models;

public class HogwartsContext : DbContext
{
    public const int MaxIngredientsForPotions = 5;
    private Random _random = new Random();

    public HogwartsContext(DbContextOptions<HogwartsContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Potion> Potions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<Room>().ToTable("Room");
        modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
        modelBuilder.Entity<Recipe>().ToTable("Recipe");
        modelBuilder.Entity<Potion>().ToTable("Potion");
    }

    
    //public async Task<Student> AddStudent(string name)
    //{
    //    HouseType houseType = (HouseType) _random.Next(0, 4);
    //    PetType petType = (PetType) _random.Next(0, 3);
    //    //Room room = await GetRoom(1);
    //    Room room = new Room();
    //    Student student = new Student {Name = name, HouseType = houseType, PetType = petType, Room = room};
    //    await Students.AddAsync(student);
    //    return student;
    //}

    //public async Task<Potion> AddEmptyPotion(long id)
    //{
    //    Student student;
    //    try
    //    {
    //        student = await GetStudent(id);
    //    }
    //    catch
    //    {
    //        student = await AddStudent($"{id}'s name");
    //    }

    //    Potion potion = new Potion
    //    {
    //        BrewingStatus = BrewingStatus.Brew,
    //        Student = student
    //    };

    //    await Potions.AddAsync(potion);
    //    await SaveChangesAsync();
    //    return potion;
    //}

    public async Task<Potion> AddIngredient(long id, Ingredient newIngredient)
    {
        Ingredient ingredient;
        try
        {
            ingredient = await GetIngredient(newIngredient.Name);
        }
        catch
        {
            ingredient = await AddNewIngredient(newIngredient.Name);
        }

        Potion potion = await GetPotion(id);
        potion.Ingredients.Add(ingredient);
        await SaveChangesAsync();
        return potion;

    }

    public async Task<Ingredient> AddNewIngredient(string name)
    {
        Ingredient ingredient = new Ingredient() { Name = name };
        await Ingredients.AddAsync(ingredient);
        return ingredient;
    }

    public async Task<List<Recipe>> GetHelp(long id)
    {
        Potion potion = await GetPotion(id);

        return Recipes
            .AsEnumerable()
            .Where(r => r.Ingredients.SequenceEqual(potion.Ingredients)).ToList();
    }

    public async Task<Potion> AddPotion(Potion potion)
    {
        if (potion.Ingredients.Any())
        {
            var newPotion = new Potion() { Student = potion.Student, Name = potion.Name };
                
            foreach (var ingredient in potion.Ingredients)
            {
                if (!Ingredients.Any(i => i.Name == ingredient.Name))
                {
                    await Ingredients.AddAsync(ingredient);
                    newPotion.Ingredients.Add(ingredient);
                }
                else
                {
                    var existingIngredient = await Ingredients.Where(i => i.Name == ingredient.Name).FirstAsync();
                    newPotion.Ingredients.Add(existingIngredient);
                }
            }

            if (newPotion.Ingredients.Count == MaxIngredientsForPotions)
            {
                if (Recipes.AsEnumerable().Any(r => r.Ingredients.SequenceEqual(newPotion.Ingredients)))
                {
                    newPotion.BrewingStatus = BrewingStatus.Replica;
                }
                else
                {
                    newPotion.BrewingStatus = BrewingStatus.Discovery;

                    int counter = GetStudentPotions(newPotion.Student.ID).Result.Count + 1;

                    var discoveredRecipe = new Recipe() { Student = newPotion.Student, Name = $"{newPotion.Student.Name}'s discovery #{counter}", Ingredients = newPotion.Ingredients, };
                    await AddAsync(discoveredRecipe);
                    newPotion.Recipe = discoveredRecipe;
                }
            }
            await Potions.AddAsync(newPotion);
            await SaveChangesAsync();
            return newPotion;
        }
        else
        {
            await Potions.AddAsync(potion);
            await SaveChangesAsync();
            return potion;
        }
    }

    public async Task<Potion> GetPotion(long potionId)
    {
        Task<Potion> potion = Potions.FindAsync(potionId).AsTask();
        return await potion;
    }



    public async Task<Ingredient> GetIngredient(string ingredientName)
    {
        Task<Ingredient> ingredient = Ingredients.FirstAsync(ingredient => ingredient.Name == ingredientName);
        return await ingredient;
    }




    public async Task<List<Potion>> GetStudentPotions(long studentId)
    {
        return await Potions
            .Where(potion => potion.Student.ID == studentId && potion.BrewingStatus == BrewingStatus.Discovery)
            .ToListAsync();
    }

    public async Task<List<Potion>> GetAllPotions()
    {
        Task<List<Potion>> potionList = Potions.ToListAsync();
        return await potionList;
    }
}