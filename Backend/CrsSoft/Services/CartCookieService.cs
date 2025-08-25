using CrsSoft.Interfaces;

namespace CrsSoft.Services
{
    public class CartCookieService : ICartCookieService
    {
        public string GetOrCreate(HttpContext http)
        {
            var cartId = http.Session.GetString("CartId");
            if (!string.IsNullOrWhiteSpace(cartId))
            {
                return cartId;
            }

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
            http.Session.SetString("CartId", cartId);
        }

        public void Clear(HttpContext http)
        {
            http.Session.Remove("CartId");
        }
    }
}
