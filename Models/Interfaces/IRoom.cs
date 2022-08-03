using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IRoom
{
    public Task<Room> GetRoom(long roomId);
    public Task<List<Room>> GetAllRooms();
    public Task AddRoom(Room room);
    public void UpdateRoom(Room room);
    public Task DeleteRoom(long id);
    public Task<List<Room>> GetAvailableRooms();
    public Task<List<Room>> GetRoomsForRatOwners();
}