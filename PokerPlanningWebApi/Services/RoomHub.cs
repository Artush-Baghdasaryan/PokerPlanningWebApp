using Microsoft.AspNetCore.SignalR;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Services;

public class RoomHub : Hub, IRoomHub
{
    private readonly IRoomService _roomService;
    private readonly IGuestService _guestService;
    
    public RoomHub(
        IRoomService roomService,
        IGuestService guestService)
    {
        _roomService = roomService;
        _guestService = guestService;
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
    
    public async Task<Guest> AddGuest(string roomId)
    {
        var guest = await _roomService.AddGuest(roomId, Context.ConnectionId);
        var room = await _roomService.GetById(roomId);
        var connectionIds = room.Guests?.Select(g => g.ConnectionId);
        if (connectionIds != null) await Clients.Clients(connectionIds!).SendAsync("GuestsUpdate");
        return guest;
    }

    public async Task GuestQuit(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("GuestsUpdate", null);
    }

    public async Task<Guest> GuestVote(string guestId, string roomId, int? score)
    {
        var guest = await _guestService.UpdateScore(guestId, score);
        var connectionIds = await GuestsConnectionIds(roomId, guest.ConnectionId);
        if (connectionIds != null) await Clients.Clients(connectionIds!).SendAsync("GuestVote", guest);
        return guest;
    }

    public async Task Reveal(string roomId)
    {
        await Clients.Group(roomId).SendAsync("Reveal");
    }

    public async Task VoteReset(string roomId)
    {
        await Clients.Group(roomId).SendAsync("VoteReset");
    }

    private async Task<IEnumerable<string?>?> GuestsConnectionIds(string roomId, string? exceptConnectionId)
    {
        var room = await _roomService.GetById(roomId);
        var connectionIds = room.Guests?.Select(g => g.ConnectionId).Where(c => c != exceptConnectionId);
        return connectionIds;
    }
}