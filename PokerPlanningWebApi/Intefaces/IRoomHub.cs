using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Intefaces;

public interface IRoomHub
{
    string GetConnectionId();
    Task<Guest> AddGuest(string roomId);
    Task GuestQuit(string roomId);
    Task<Guest> GuestVote(string guestId, string roomId, int? score);
    Task Reveal(string roomId);
    Task VoteReset(string roomId);
}