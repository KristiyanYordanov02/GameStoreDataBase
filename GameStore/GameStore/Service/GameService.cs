using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Interfaces;
using GameStore.Core.Models;

namespace GameStore.Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repository;

        public GameService(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Game>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Game> GetByIdAsync(string id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Game game) =>
            await _repository.AddAsync(game);

        public async Task UpdateAsync(Game game) =>
            await _repository.UpdateAsync(game);

        public async Task DeleteAsync(string id) =>
            await _repository.DeleteAsync(id);
    }
}