using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using GameStore.Infrastructure.Data;
using MongoDB.Driver;

namespace GameStore.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> _games;

        public GameRepository(MongoDBContext context)
        {
            _games = context.Games; // <- това ще работи ако Games е деклариран както горе
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _games.Find(_ => true).ToListAsync();
        }

        public async Task<Game> GetByIdAsync(string id)
        {
            return await _games.Find(g => g.Id.ToString() == id).FirstOrDefaultAsync(); // Преобразуваме Guid към string
        }

        public async Task AddAsync(Game game)
        {
            await _games.InsertOneAsync(game);
        }

        public async Task UpdateAsync(Game game)
        {
            await _games.ReplaceOneAsync(g => g.Id == game.Id, game);
        }

        public async Task DeleteAsync(string id)
        {
            await _games.DeleteOneAsync(g => g.Id.ToString() == id); // отново Guid -> string
        }
    }
}