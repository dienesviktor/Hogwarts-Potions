using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Implementations;

public class RoomImplementation : IRoom
{
    private readonly HogwartsContext _context;

    public RoomImplementation(HogwartsContext context)
    {
        _context = context;
    }

    public async Task<Room> GetRoom(long roomId)
    {
        return await _context.Rooms
            .Include(r => r.Residents)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ID == roomId);
    }

    public async Task<List<Room>> GetAllRooms()
    {
        return await _context.Rooms
            .Include(r => r.Residents)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddRoom(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public void UpdateRoom(Room room)
    {
        _context.Rooms.Update(room);
        _context.SaveChanges();
    }

    public async Task DeleteRoom(long id)
    {
        Room room = GetRoom(id).Result;

        await _context.Students
            .Where(s => s.Room == room)
            .LoadAsync();
   
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAvailableRooms()
    {
        return await _context.Rooms
            .Include(r => r.Residents)
            .AsNoTracking()
            .Where(room => room.Residents.Count < room.Capacity)
            .ToListAsync();
    }

    public async Task<List<Room>> GetRoomsForRatOwners()
    {
        return await _context.Rooms
            .Include(r => r.Residents)
            .AsNoTracking()
            .Where(room => !room.Residents.Any(resident => resident.PetType == PetType.Cat || resident.PetType == PetType.Owl))
            .ToListAsync();
    }
}