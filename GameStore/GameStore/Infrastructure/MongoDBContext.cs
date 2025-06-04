using GameStore.Core.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace GameStore.Infrastructure.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("GameStoreDb"); // използвай реалното име на твоята база
        }

        public IMongoCollection<Game> Games => _database.GetCollection<Game>("Games");
    }
}