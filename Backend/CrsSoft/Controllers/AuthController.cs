using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrsSoft.Controllers
{
    [Route("/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService authService;
        readonly ICartService cartService;
        readonly ICartCookieService cartCookieService;

        public AuthController(IAuthService authService, ICartService cartService, ICartCookieService cartCookieService)
        {
            this.authService = authService;
            this.cartService = cartService;
            this.cartCookieService = cartCookieService;
        }

        // Kullanıcıların siteye kendi hesaplarıyla girişini sağlıyor
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequestModel request)
        {
            try
            {
                var anonymousCartId = cartCookieService.GetOrCreate(HttpContext);

                var result = await authService.LoginUser(request);

                if (Guid.TryParse(anonymousCartId, out var cartGuid) && cartGuid != Guid.Empty)
                {
                    await cartService.MergeCarts(cartGuid, result.UserId);
                }

                var userCart = await cartService.GetOrCreateForUser(result.UserId);
                cartCookieService.SetCartId(HttpContext, userCart.Id.ToString("N"));

                HttpContext.Session.SetString("auth_token", result.AuthToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid Email or Password.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Missing field: {ex.ParamName}");
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred during login: {ex.Message}");
            }
        }

        // Kullanıcıların hesap oluşturmasını sağlıyor
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestModel request)
        {
            try
            {
                await authService.RegisterUser(request);
                return Ok("Registration successful.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Missing field: {ex.ParamName}");
            }
            catch (InvalidOperationException)
            {
                return Conflict("A user with this email already exists."); 
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred during login: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult LogoutUser()
        {
            try
            {
                HttpContext.Session.Remove("auth_token");
                cartCookieService.CreateNewCartCookie(HttpContext);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred during logout: {ex.Message}");
            }
        }

        [HttpGet("isAuthenticated")]
        public bool IsAuthenticated()
        {
            var authToken = HttpContext.Session.GetString("auth_token");
            return !string.IsNullOrEmpty(authToken);
        }
    }
}
