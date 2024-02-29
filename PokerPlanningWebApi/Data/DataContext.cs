using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PokerPlanningWebApi.Models;

namespace PokerPlanningWebApi.Data;

public class DataContext
{
    private IMongoDatabase _database { get; set; }
    private IMongoClient _client { get; set; }

    public DataContext(IOptions<MongoSettings> settings)
    {
        _client = new MongoClient(settings.Value.ConnectionString);
        _database = _client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
    {
        var collection = _database.GetCollection<TEntity>(collectionName);
        return collection;
    }
}