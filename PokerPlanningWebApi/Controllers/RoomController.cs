using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;
using PokerPlanningWebApi.Services;

namespace PokerPlanningWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IGuestService _guestService;
    private readonly IHubContext<RoomHub> _hubContext;

    public RoomController(
        IRoomService roomService,
        IGuestService guestService,
        IHubContext<RoomHub> hubContext)
    {
        _roomService = roomService;
        _guestService = guestService;
        _hubContext = hubContext;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<Room>>> GetAll()
    {
        try
        {
            var rooms = await _roomService.GetAll();
            return Ok(rooms);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<Room>> GetById(string roomId)
    {
        try
        {
            var room = await _roomService.GetById(roomId);
            return Ok(room);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("get-guests/{roomId}")]
    public async Task<ActionResult<List<Guest>?>> GetGuests(string roomId)
    {
        try
        {
            var guests = await _roomService.GetGuests(roomId);
            return Ok(guests);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create-new-room")]
    public async Task<ActionResult<Room>> CreateRoom([FromQuery] string roomName)
    {
        try
        {
            var room = await _roomService.CreateRoom(roomName);
            return Ok(room);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{roomId}")]
    public async Task<ActionResult> DeleteRoom(string roomId)
    {
        try
        {
            await _roomService.DeleteRoom(roomId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-guest/{roomId}")]
    public async Task<ActionResult<Guest>> AddGuestToRoom(string roomId)
    {
        try
        {
            var guest = await _roomService.AddGuest(roomId);
            var room = await _roomService.GetById(roomId);
            /*var guestsIds = room.Guests.Where(g => g.Id != guest.Id).Select(g => g.Id);
            await _hubContext.Clients
                .All
                .SendAsync("GuestJoined", guest);*/
            return Ok(guest);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("remove-guest/{roomId}")]
    public async Task<ActionResult> RemoveGuest(string roomId, [FromQuery] string guestId)
    {
        try
        {
            await _roomService.RemoveGuest(roomId, guestId);
            return Ok(guestId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}