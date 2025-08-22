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

            // Create new ID if none exists
            return CreateNewCartCookie(http);
        }

        public string CreateNewCartCookie(HttpContext http)
        {
            var newId = Guid.NewGuid().ToString("N");
            SetCartId(http, newId);
            return newId;
        }

        public void SetCartId(HttpContext http, string cartId)
        {
            http.Response.Cookies.Append(CookieName, cartId, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                Path = "/"
            });
        }

        public void Clear(HttpContext http)
        {
            http.Response.Cookies.Delete(CookieName);
        }
    }
}
