using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Intefaces;

public interface IGuestService
{
    Task<List<Guest>> GetAll();
    Task<Guest> GetById(string guestId);
    Task<Guest> CreateGuest(int index, string? connectionId = null);
    Task RemoveGuest(string guestId);
    Task<Guest> UpdateScore(string guestId, int? score);
}