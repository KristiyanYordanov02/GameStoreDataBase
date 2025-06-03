using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Models;

namespace GameStore.Core.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(string id);
        Task AddAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(string id);
    }
}