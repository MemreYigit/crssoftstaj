using CrsSoft.Interfaces;
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
        public async Task<IActionResult> GetAllGames()
        {
            try
            {
                var games = await gameService.GetAllGames();
                if (games == null)
                {
                    return NotFound("There is no games to sell");
                }
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred while retrieving the games: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleGame(int id)
        {
            try
            {
                var game = await gameService.GetSingleGame(id);
                if (game == null)
                {
                    return NotFound("Game is not found");
                }
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred while retrieving the game: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchGames([FromQuery] string? s)
        {
            try
            {
                var games = await gameService.SearchGames(s);
                if (games == null)
                {
                    return NotFound("No games found");
                }
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred while searching for games: {ex.Message}");
            }
        }
    }
}