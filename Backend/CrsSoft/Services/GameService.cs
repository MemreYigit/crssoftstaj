using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrsSoft.Services
{
    public class GameService : IGameService
    {
        private readonly DataContext _dataContext;

        public GameService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Game>> GetAllGames()
        {
            try
            {
                var games = await _dataContext.Games.ToListAsync();
                return games;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Game> GetSingleGame(int id)
        {
            try
            {
                var game = await _dataContext.Games.FindAsync(id);
                return game;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Game>> SearchGames(string? s)
        {
            try
            {
                var games = _dataContext.Games.AsQueryable(); // I can use "Where" with AsQueryable

                if (!string.IsNullOrWhiteSpace(s))
                {
                    games = games.Where(g => g.Name.Contains(s));
                }

                return await games.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }   
}
