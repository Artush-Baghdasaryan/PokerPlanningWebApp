using Microsoft.CodeAnalysis.Elfie.Serialization;
using MongoDB.Bson;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;
using PokerPlanningWebApi.Rpositories;

namespace PokerPlanningWebApi.Services;

public class GuestService : IGuestService
{
    private readonly GuestRepository _guestRepository;

    public GuestService(GuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
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

    public async Task<Guest> CreateGuest(int index, string? connectionId = null)
    {
        var guest = new Guest
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Score = null,
            Index = index,
            ConnectionId = connectionId
        };
        await _guestRepository.AddEntity(guest);
        return guest;
    }

    public async Task RemoveGuest(string guestId)
    {
        await _guestRepository.Delete(guestId);
    }

    public async Task<Guest> UpdateScore(string guestId, int? score)
    {
        var guest = await _guestRepository.GetById(guestId);
        if (guest == null)
        {
            throw new Exception("Guest was not found");
        }
        guest.Score = score;
        await _guestRepository.UpdateEntity(guestId, guest);
        return guest;
    }
    
}