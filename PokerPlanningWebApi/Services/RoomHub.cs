using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
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
        await SendMessage(roomId, "GuestsUpdate");
        return guest;
    }

    public async Task GuestQuit(string roomId)
    {
        var connectionIds = await GuestsConnectionIds(roomId);
        if (connectionIds != null) await Clients.Clients(connectionIds!).SendAsync("GuestsUpdate");
    }

    public async Task<Guest> GuestVote(string guestId, string roomId, int? score)
    {
        var guest = await _guestService.UpdateScore(guestId, score);
        await SendMessage(roomId, "GuestVote", guest);
        return guest;
    }

    public async Task Reveal(string roomId)
    {
        var score = await _roomService.Reveal(roomId);
        await SendMessage(roomId, "Reveal", score);
    }

    public async Task ResetVoting(string roomId)
    {
        await _roomService.ResetVoting(roomId);
        await SendMessage(roomId, "ResetVoting");
    }

    private async Task<IEnumerable<string?>?> GuestsConnectionIds(string roomId, string? exceptConnectionId = null)
    {
        var guests = await _roomService.GetGuests(roomId);
        var connectionIds = guests.Select(g => g.ConnectionId).Where(c => c != exceptConnectionId);
        return connectionIds;
    }

    private async Task SendMessage(string roomId, string command, params Object[] messages)
    {
        var guests = await _roomService.GetGuests(roomId);
        var connectionIds = guests?.Select(g => g.ConnectionId);
        if (connectionIds == null || !connectionIds!.Any()) return;
        await Clients.Clients(connectionIds!).SendAsync(command, messages);
    }
}