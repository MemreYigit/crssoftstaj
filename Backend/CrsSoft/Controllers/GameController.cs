using CrsSoft.Entities.Enums;
using CrsSoft.Interfaces;
using CrsSoft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrsSoft.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllGames()
        {
            var games = await gameService.GetAllGamesAsync();
            if (games == null)
            {
                return NotFound("There is no games to sell");
            }
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSingleGame(int id)
        {
            var game = await gameService.GetSingleGameAsync(id);
            if (game == null)
            {
                return NotFound("Game is not found");
            }
            return Ok(game);
        }   

        [HttpGet("search")]
        public async Task<ActionResult> SearchGames([FromQuery] string? s)
        {
            var games = await gameService.SearchGamesAsync(s);
            if (games == null)
            {
                return NotFound("No games found");
            }
            return Ok(games);
        }
    }
}