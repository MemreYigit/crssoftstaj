using CrsSoft.Interfaces;
using CrsSoft.Models;
using CrsSoft.Services;
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
        public async Task<ActionResult> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            try
            {
                var anonymousCartId = cartCookieService.GetOrCreate(HttpContext);

                var result = await authService.LoginUserAsync(request);

                // If we have a valid anonymous cart ID, merge it with user's cart
                if (Guid.TryParse(anonymousCartId, out var cartGuid) && cartGuid != Guid.Empty)
                {
                    await cartService.MergeCarts(cartGuid, result.UserId);
                }

                // Get the user's actual cart and update the cookie
                var userCart = await cartService.GetOrCreateForUser(result.UserId);
                cartCookieService.SetCartId(HttpContext, userCart.Id.ToString("N"));

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                };

                Response.Cookies.Append("AuthToken", result.AuthToken, cookieOptions);

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
        public async Task<ActionResult> RegisterUserAsync([FromBody] UserRegisterRequest request)
        {
            try
            {
                await authService.RegisterUserAsync(request);
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
        public ActionResult LogoutUserAsync()
        {
            try
            {
                // Clear authentication cookie
                Response.Cookies.Delete("AuthToken");

                // Create a completely new anonymous cart
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
            var authToken = Request.Cookies["AuthToken"];
            return !string.IsNullOrEmpty(authToken);
        }
    }
}
