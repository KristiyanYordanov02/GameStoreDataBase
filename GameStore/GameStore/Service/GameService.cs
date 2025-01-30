using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using Mapster;

namespace GameStore.Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> AddGame(GameDto gameDto)
        {
            var game = gameDto.Adapt<Game>();
            game.Id = Guid.NewGuid();
            return await _gameRepository.AddGame(game);
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _gameRepository.GetAllGames();
        }
    }
}