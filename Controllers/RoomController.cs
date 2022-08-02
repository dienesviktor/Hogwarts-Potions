using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models;
using HogwartsPotions.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("/room")]
public class RoomController : ControllerBase
{
    private readonly HogwartsContext _context;

    public RoomController(HogwartsContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Room>> GetAllRooms()
    {
        return await _context.GetAllRooms();
    }

    [HttpPost]
    public async Task AddRoom([FromBody] Room room)
    {
        await _context.AddRoom(room);
        await _context.SaveChangesAsync();
    }

    [HttpGet("/room/{id}")]
    public async Task<Room> GetRoomById(long id)
    {
        return await _context.GetRoom(id);
    }

    [HttpPut("/room/{id}")]
    public void UpdateRoomById(long id, [FromBody] Room updatedRoom)
    {
        updatedRoom.ID = id;
        _context.UpdateRoom(updatedRoom);
    }

    [HttpDelete("/room/{id}")]
    public async Task DeleteRoomById(long id)
    {
        await _context.DeleteRoom(id);
        await _context.SaveChangesAsync();
    }

    [HttpGet("/room/available")]
    public async Task<List<Room>> GetAvailableRooms()
    {
        return await _context.GetAvailableRooms();
    }

    [HttpGet("/room/rat-owners")]
    public async Task<List<Room>> GetRoomsForRatOwners()
    {
        return await _context.GetRoomsForRatOwners();
    }
}