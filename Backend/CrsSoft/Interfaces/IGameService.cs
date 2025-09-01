using CrsSoft.Entities;
using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetAllGames();
        Task<Game> GetSingleGame(int id);
        Task<List<Game>> SearchGames(string? s);
    }
}
