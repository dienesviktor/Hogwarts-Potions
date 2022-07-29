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

            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var rooms = new Room[]
            {
                new Room{Capacity = 5, Residents = new HashSet<Student> {new Student{Name = "Harry Potter", HouseType = HouseType.Gryffindor, PetType = PetType.Owl}}},
                new Room{Capacity = 5, Residents = new HashSet<Student> {new Student{Name = "Hermione Granger", HouseType = HouseType.Gryffindor, PetType = PetType.Cat}}}
            };
            foreach (Room r in rooms)
            {
                context.Rooms.Add(r);
            }
            context.SaveChanges();

        }
    }
}