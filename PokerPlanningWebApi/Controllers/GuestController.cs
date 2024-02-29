using Microsoft.AspNetCore.Mvc;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;
using PokerPlanningWebApi.Rpositories;
using PokerPlanningWebApi.Services;

namespace PokerPlanningWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GuestController : ControllerBase
{
    private readonly IGuestService _guestService;

    public GuestController(IGuestService guestService)
    {
        _guestService = guestService;
    }
    
    [HttpGet("get-all")]
    public async Task<ActionResult<List<Guest>>> GetAll()
    {
        try
        {
            var guests = await _guestService.GetAll();
            return Ok(guests);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{guestId}")]
    public async Task<ActionResult<Guest>> GetById(string guestId)
    {
        try
        {
            var guest = await _guestService.GetById(guestId);
            return Ok(guest);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> CraeteGuest([FromQuery]string? roomId)
    {
        try
        {
            var guestId = await _guestService.CreateGuest(roomId);
            return Ok(guestId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{guestId}")]
    public async Task<ActionResult> RemoveGuest(string guestId)
    {
        try
        {
            await _guestService.RemoveGuest(guestId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("score/{guestId}")]
    public async Task<ActionResult> UpdateScore(string guestId, int score)
    {
        try
        {
            await _guestService.UpdateScore(guestId, score);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}