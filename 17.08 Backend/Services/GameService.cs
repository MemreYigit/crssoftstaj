using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<List<Game>> GetAllGamesAsync()
        {
            var games = await _dataContext.Games.ToListAsync();

            return games;   
        }

        public async Task<Game> GetSingleGameAsync(int id)
        {
            var game = await _dataContext.Games.FindAsync(id);
            return game;
        }

        public async Task<List<Game>> SearchGamesAsync(string? s)
        {
            var games = _dataContext.Games.AsQueryable(); // I can use "Where" with AsQueryable

            if (!string.IsNullOrWhiteSpace(s))
            {
                games = games.Where(g => g.Name.Contains(s));
            }

            return await games.ToListAsync();
        }
    }   
}
