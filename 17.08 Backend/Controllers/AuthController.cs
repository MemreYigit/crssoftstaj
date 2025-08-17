using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrsSoft.Controllers
{
    [Route("/")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        readonly IAuthService authService;

        public AuthController(IAuthService authService) 
        { 
            this.authService = authService;
        }

        // Kullanıcıların siteye kendi hesaplarıyla girişini sağlıyor
        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            try
            {
                var result = await authService.LoginUserAsync(request);
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
    }
}
