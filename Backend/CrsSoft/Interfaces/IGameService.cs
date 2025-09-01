using CrsSoft.Entities;
using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetAllGamesAsync();
        Task<Game> GetSingleGameAsync(int id);
        Task<List<Game>> SearchGamesAsync(string? s);
    }
}
