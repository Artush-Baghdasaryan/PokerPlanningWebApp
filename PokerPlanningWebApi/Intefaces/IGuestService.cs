using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Intefaces;

public interface IGuestService
{
    Task<List<Guest>> GetAll();
    Task<Guest> GetById(string guestId);
    Task<string> CreateGuest(string? roomId);
    Task RemoveGuest(string guestId);
    Task UpdateScore(string guestId, int score);
}