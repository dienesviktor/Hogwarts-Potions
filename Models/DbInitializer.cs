using System.Collections.Generic;
using System.Linq;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Models
{
    public static class DbInitializer
    {
        public static void Initialize(HogwartsContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any() || context.Rooms.Any() || context.Potions.Any())
            {
                return;
            }

            var rooms = new Room[]
            {
                new Room{Capacity = 5, Residents = new HashSet<Student> {new Student{Name = "Harry Potter", HouseType = HouseType.Gryffindor, PetType = PetType.Owl}}},
                new Room{Capacity = 5, Residents = new HashSet<Student> {new Student{Name = "Hermione Granger", HouseType = HouseType.Gryffindor, PetType = PetType.Rat}}}
            };

            foreach (Room r in rooms)
            {
                context.Rooms.Add(r);
            }

            var potions = new Potion[]
            {
                new Potion()
                {
                    BrewingStatus = BrewingStatus.Brew,
                    Name = "potionke",
                    Student = new Student(){Name = "Harry Potter", HouseType = HouseType.Gryffindor, PetType = PetType.Owl},
                    Ingredients = {new Ingredient {Name = "apple"}}
                }
            };

            foreach (Potion p in potions)
            {
                context.Potions.Add(p);
            }

            context.SaveChanges();
        }
    }
}