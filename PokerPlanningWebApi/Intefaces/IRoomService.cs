using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Intefaces;

public interface IRoomService
{
    Task<List<Room>> GetAll();
    Task<Room> GetById(string roomId);
    Task<IList<Guest>?> GetGuests(string roomId);
    Task<Room> CreateRoom(string roomName);
    Task DeleteRoom(string roomId);
    Task<Guest> AddGuest(string roomId, string? connectionId = null);
    Task RemoveGuest(string roomId, string guestId);
    Task<int?> Reveal(string roomId);
}