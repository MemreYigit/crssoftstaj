using Microsoft.Net.Http;

namespace CrsSoft.Services
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int? expireTime = null)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            
            if (expireTime.HasValue)
            {
                options.Expires = DateTimeOffset.UtcNow.AddMinutes(expireTime.Value);
            }

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
        }

        public string? GetCookie(string key)
        {   
            return _httpContextAccessor.HttpContext?.Request.Cookies[key];
        }

        public void DeleteCookie(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }
    }
}