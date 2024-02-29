using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Intefaces;

public interface IRoomService
{
    Task<List<Room>> GetAll();
    Task<Room> GetById(string roomId);
    Task<string> CreateRoom(string adminId, string roomName);
    Task DeleteRoom(string roomId);
    Task<string> AddGuest(string roomId, string guestId);
    Task RemoveGuest(string roomId, string guestId);
}