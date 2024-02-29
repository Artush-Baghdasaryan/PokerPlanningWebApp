using MongoDB.Bson;
using MongoDB.Driver;
using PokerPlanningWebApi.Data;
using PokerPlanningWebApi.Intefaces;

namespace PokerPlanningWebApi.Rpositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context;
    private readonly IMongoCollection<TEntity> _collection;
    
    public BaseRepository(DataContext dataContext, string collectionName)
    {
        _context = dataContext;
        _collection = _context.GetCollection<TEntity>(collectionName);
    }

    public async Task<TEntity?> GetById(string id)
    {
        return await _collection.FindAsync(GetObjectId(id)).Result.FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAll()
    {
        var allEntities = await _collection.FindAsync(Builders<TEntity>.Filter.Empty);
        return await allEntities.ToListAsync();    }

    public async Task<bool> DoesExist(string id)
    {
        var exists = await GetById(id);
        return exists != null;    }

    public async Task AddEntity(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateEntity(string id, TEntity entity)
    {
        await _collection.ReplaceOneAsync(GetObjectId(id), entity);
    }

    public async Task Delete(string id)
    {
        await _collection.DeleteOneAsync(GetObjectId(id));
    }
    
    private FilterDefinition<TEntity> GetObjectId(string id)
    {
        var objectId = new ObjectId(id);
        return Builders<TEntity>.Filter.Eq("_id", objectId);
    }
}