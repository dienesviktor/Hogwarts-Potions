using System.Collections.Generic;
using System.Linq;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Models;

public static class DbInitializer
{
    public static void Initialize(HogwartsContext context)
    {
        context.Database.EnsureCreated();

        if (context.Students.Any() || context.Rooms.Any() || context.Potions.Any() || context.Potions.Any())
        {
            return;
        }

        Student Harry = new Student
            {Name = "Harry Potter", HouseType = HouseType.Gryffindor, PetType = PetType.Owl};

        Student Herminone = new Student
            {Name = "Hermione Granger", HouseType = HouseType.Gryffindor, PetType = PetType.Rat};

        Ingredient apple = new Ingredient{Name = "apple"};
        Ingredient banana = new Ingredient {Name = "banana"};
        Ingredient orange = new Ingredient {Name = "orange"};
        Ingredient mango = new Ingredient {Name = "mango"};
        Ingredient lime = new Ingredient {Name = "lime"};

        Recipe perfectionSalad = new Recipe
        {
            Name = "Perfection Salad",
            Ingredients = new HashSet<Ingredient> {apple, banana, orange},
            Student = Harry
        };

        Recipe oneIngredient = new Recipe
        {
            Name = "One ingredient",
            Ingredients = new HashSet<Ingredient> {mango},
            Student = Herminone
        };

        Potion potionOne = new Potion()
        {
            BrewingStatus = BrewingStatus.Brew,
            Name = "Potion one",
            Student = Harry,
            Ingredients = {apple, banana, orange},
            Recipe = perfectionSalad
        };

        Potion potionTwo = new Potion()
        {
            BrewingStatus = BrewingStatus.Brew,
            Name = "Potion two",
            Student = Herminone,
            Ingredients = {mango},
            Recipe = oneIngredient
        };

        var rooms = new Room[]
        {
            new Room{Capacity = 5, Residents = new HashSet<Student> {Harry}},
            new Room{Capacity = 5, Residents = new HashSet<Student> {Herminone}}
        };
            
        foreach (Room r in rooms)
        {
            context.Rooms.Add(r);
        }

        var recipes = new Recipe[]
        {
            perfectionSalad,
            oneIngredient
                
        };

        foreach (Recipe r in recipes)
        {
            context.Recipes.Add(r);
        }

        var potions = new Potion[]
        {
            potionOne,
            potionTwo
        };

        foreach (Potion p in potions)
        {
            context.Potions.Add(p);
        }

        context.SaveChanges();
    }
}