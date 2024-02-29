using Microsoft.CodeAnalysis.Elfie.Serialization;
using MongoDB.Bson;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;
using PokerPlanningWebApi.Rpositories;

namespace PokerPlanningWebApi.Services;

public class GuestService : IGuestService
{
    private readonly GuestRepository _guestRepository;
    private readonly RoomRepository _roomRepository;
    private readonly IRoomService _roomService;

    public GuestService(GuestRepository guestRepository, RoomRepository roomRepository, IRoomService roomService)
    {
        _guestRepository = guestRepository;
        _roomRepository = roomRepository;
        _roomService = roomService;
    }

    public async Task<List<Guest>> GetAll()
    {
        var guests = await _guestRepository.GetAll();
        return guests;
    }

    public async Task<Guest> GetById(string guestId)
    {
        var guest = await _guestRepository.GetById(guestId);
        if (guest == null) throw new Exception("Guest was not found");
        return guest;
    }

    public async Task<string> CreateGuest(string? roomId)
    {
        if (roomId == null)
        {
            var adminId = await CreateAdmin();
            return adminId;
        } 
        
        var room = await _roomRepository.GetById(roomId);
        if (room == null) throw new Exception("Room was not found!");
        var index = room.Guests!.Count + 1; // +1 for new index of guest
        var guest = new Guest
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Score = 0,
            Index = index
        };
        await _guestRepository.AddEntity(guest);
        await _roomService.AddGuest(roomId, guest.Id);
        return guest.Id;
    }

    private async Task<string> CreateAdmin()
    {
        var admin = new Guest
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Score = 0,
            Index = 1
        };
        await _guestRepository.AddEntity(admin);
        return admin.Id;
    }

    public async Task RemoveGuest(string guestId)
    {
        await _guestRepository.Delete(guestId);
    }

    public async Task UpdateScore(string guestId, int score)
    {
        var guest = await _guestRepository.GetById(guestId);
        if (guest == null)
        {
            return;
        }
        guest.Score = score;
        await _guestRepository.UpdateEntity(guestId, guest);
    }
}