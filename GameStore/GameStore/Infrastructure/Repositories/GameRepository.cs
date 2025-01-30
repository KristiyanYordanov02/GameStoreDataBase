using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using MongoDB.Driver;

namespace GameStore.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> _games;

        public GameRepository(IMongoDatabase database)
        {
            _games = database.GetCollection<Game>("Games");
        }

        public async Task<Game> AddGame(Game game)
        {
            await _games.InsertOneAsync(game);
            return game;
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _games.Find(_ => true).ToListAsync();
        }
    }
}