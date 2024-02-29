using MongoDB.Driver;
using PokerPlanningWebApi.Data;
using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Rpositories;

public class GuestRepository : BaseRepository<Guest>
{
    private readonly DataContext _context;
    private readonly IMongoCollection<Guest> _collection;
    
    public GuestRepository(DataContext context) : base(context, "guests")
    {
        _context = context;
        _collection = _context.GetCollection<Guest>("guests");
    }
}