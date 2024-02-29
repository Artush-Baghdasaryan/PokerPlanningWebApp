using Microsoft.AspNetCore.Mvc;
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

    public RoomController(IRoomService roomService, IGuestService guestService)
    {
        _roomService = roomService;
        _guestService = guestService;
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

    [HttpPost("create-new-room")]
    public async Task<ActionResult<string>> CreateRoom([FromQuery] string adminId, [FromQuery] string roomName)
    {
        try
        {
            var roomId = await _roomService.CreateRoom(adminId, roomName);
            return Ok(roomId);
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
    public async Task<ActionResult> AddGuest(string roomId, [FromQuery]string guestId)
    {
        try
        {
            await _roomService.AddGuest(roomId, guestId);
            return Ok();
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