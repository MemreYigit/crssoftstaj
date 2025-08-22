using CrsSoft.Entities;
using CrsSoft.Models;

namespace CrsSoft.Interfaces
{
    public interface IGameService
    {
        public Task<List<Game>> GetAllGamesAsync();
        public Task<Game> GetSingleGameAsync(int id);
        public Task<List<Game>> SearchGamesAsync(string? s);
    }
}
