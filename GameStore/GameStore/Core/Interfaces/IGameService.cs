using GameStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Core.Interfaces
{
    public interface IGameService
    {
        Task<Game> AddGame(GameDto gameDto);
        Task<IEnumerable<Game>> GetAllGames();
    }
}