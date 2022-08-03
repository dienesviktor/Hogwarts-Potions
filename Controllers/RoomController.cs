using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("/room")]
public class RoomController : ControllerBase
{
    private readonly IRoom _roomImplementation;

    public RoomController(IRoom room)
    {
        _roomImplementation = room;
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<Room>> GetRoom(long roomId)
    {
        Room room = await _roomImplementation.GetRoom(roomId);
        if (room is null)
        {
            return NotFound($"Room #{roomId} doesn't exist!");
        }
        return Ok(room);
    }

    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetAllRooms()
    {
        List<Room> rooms = await _roomImplementation.GetAllRooms();
        if (rooms.Count == 0)
        {
            return NoContent();
        }
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> AddRoom([FromBody] Room room)
    {
        await _roomImplementation.AddRoom(room);
        return Created("AddRoom", room);
    }

    [HttpPut("{roomId}")]
    public async Task<ActionResult> UpdateRoom(long roomId, [FromBody] Room room)
    {
        Room newRoom = await _roomImplementation.GetRoom(roomId);
        if (newRoom is null)
        {
            return NotFound($"Room #{roomId} doesn't exist!");
        }
        room.ID = newRoom.ID;
        _roomImplementation.UpdateRoom(room);
        return NoContent();
    }

    [HttpDelete("{roomId}")]
    public async Task<ActionResult> DeleteRoom(long roomId)
    {
        Room room = await _roomImplementation.GetRoom(roomId);
        if (room is null)
        {
            return NotFound($"Room #{roomId} doesn't exist!");
        }
        await _roomImplementation.DeleteRoom(roomId);
        return NoContent();
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<Room>>> GetAvailableRooms()
    {
        List<Room> rooms = await _roomImplementation.GetAvailableRooms();
        if (rooms.Count == 0)
        {
            return NoContent();
        }
        return Ok(rooms);
    }

    [HttpGet("for-rat-owners")]
    public async Task<ActionResult<List<Room>>> GetRoomsForRatOwners()
    {
        List<Room> rooms = await _roomImplementation.GetRoomsForRatOwners();
        if (rooms.Count == 0)
        {
            return NoContent();
        }
        return Ok(rooms);
    }
}