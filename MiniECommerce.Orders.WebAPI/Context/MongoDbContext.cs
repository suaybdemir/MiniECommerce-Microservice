using Microsoft.Extensions.Options;
using MiniECommerce.Orders.WebAPI.Options;
using MongoDB.Driver;

namespace MiniECommerce.Orders.WebAPI.Context;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}
