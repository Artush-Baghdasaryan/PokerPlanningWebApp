using MongoDB.Driver;
using PokerPlanningWebApi.Data;
using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Rpositories;

public class RoomRepository : BaseRepository<Room>
{
    private readonly DataContext _context;
    private readonly IMongoCollection<Room> _collection;

    public RoomRepository(DataContext context) : base(context, "rooms")
    {
        _context = context;
        _collection = _context.GetCollection<Room>("rooms");
    }
    
    public async Task AddGuestToRoom(Room room, Guest guest)
    {
        room.Guests!.Add(guest);
        await UpdateEntity(room.Id!, room);
    }
    
    public async Task RemoveGuestFromRoom(Room room, Guest guest)
    {
        room.Guests!.Remove(guest);
        await UpdateEntity(room.Id!, room);
    }
}