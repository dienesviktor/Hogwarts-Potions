using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HogwartsPotions.Models
{
    public class HogwartsContext : DbContext
    {
        public const int MaxIngredientsForPotions = 5;

        public HogwartsContext(DbContextOptions<HogwartsContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Room>().ToTable("Room");
        }

        public async Task AddRoom(Room room)
        {
            await Rooms.AddAsync(room);
        }

        public Task<Room> GetRoom(long roomId)
        {
            Task<Room> room = Rooms.FindAsync(roomId).AsTask();
            return room;
        }

        public async Task<List<Room>> GetAllRooms()
        {
            Task<List<Room>> roomList = Rooms.ToListAsync();
            return await roomList;
        }

        public async Task Update(long id, Room room)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRoom(long id)
        {
            Room room = GetRoom(id).Result;
            Rooms.Remove(room);
        }

        public Task<List<Room>> GetRoomsForRatOwners()
        {
            throw new NotImplementedException();
        }
    }
}
