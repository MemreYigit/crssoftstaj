using CrsSoft.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrsSoft.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly ICartCookieService cartCookieService;

        public CartController(ICartService cartService, ICartCookieService cartCookieService)
        {
            this.cartService = cartService;
            this.cartCookieService = cartCookieService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cartIdStr = cartCookieService.GetOrCreate(HttpContext);
            var cartId = Guid.Parse(cartIdStr);

            var result = await cartService.GetCartDetails(cartId);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CartRequest req)
        {
            if (req is null) return BadRequest("Invalid game.");


            var cartIdStr = cartCookieService.GetOrCreate(HttpContext);
            var cartId = Guid.Parse(cartIdStr);

            await cartService.AddOrIncrementGame(cartId, req.GameId, req.Quantity);
            return NoContent();
        }

        [HttpPost("decrement")]
        public async Task<IActionResult> Decrement([FromBody] CartRequest req)
        {
            if (req is null) return BadRequest("Invalid game.");

            var cartIdStr = cartCookieService.GetOrCreate(HttpContext);
            var cartId = Guid.Parse(cartIdStr);

            await cartService.DecrementGame(cartId, req.GameId, Math.Max(1, req.Quantity));
            return NoContent();
        }

        [HttpPost("empty")]
        public async Task<IActionResult> EmptyCart()
        {
            var cartIdStr = cartCookieService.GetOrCreate(HttpContext);
            var cartId = Guid.Parse(cartIdStr);

            await cartService.EmptyCart(cartId);
            return NoContent();
        }

        public record CartRequest(int GameId, int Quantity = 1);
    }
}
