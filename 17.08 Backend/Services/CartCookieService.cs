using CrsSoft.Interfaces;

namespace CrsSoft.Services
{
    public class CartCookieService : ICartCookieService
    {
        private const string CookieName = "cart_id";

        public string GetOrCreate(HttpContext http)
        {
            // Try to get existing cookie
            if (http.Request.Cookies.TryGetValue(CookieName, out var id) && Guid.TryParseExact(id, "N", out _))
            {
                return id;
            }

            // Create new ID if none exists or is invalid
            var newId = Guid.NewGuid().ToString("N");

            // Set the cookie with the new ID
            http.Response.Cookies.Append(CookieName, newId, new CookieOptions
            {
                HttpOnly = true,        
                Secure = false,         // Allow HTTP              
                SameSite = SameSiteMode.Lax,  
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                Path = "/"
            });
            return newId;
        }

        public void Clear(HttpContext http)
        {
            http.Response.Cookies.Delete(CookieName, new CookieOptions
            {
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
    }
}
