using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using MongoDB.Driver;

namespace GameStore.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> _games;

        public GameRepository(MongoDBContext context)
        {
            _games = context.Games;
        }

        public async Task<IEnumerable<Game>> GetAllAsync() =>
            await games.Find( => true).ToListAsync();

        public async Task<Game> GetByIdAsync(string id) =>
            await _games.Find(g => g.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Game game) =>
            await _games.InsertOneAsync(game);
        з
        public async Task UpdateAsync(Game game) =>
            await _games.ReplaceOneAsync(g => g.Id == game.Id, game);

        public async Task DeleteAsync(string id) =>
            await _games.DeleteOneAsync(g => g.Id == id);
    }
}