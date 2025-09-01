using CrsSoft.Interfaces;

namespace CrsSoft.Services
{
    public class CartCookieService : ICartCookieService
    {
        public string GetOrCreate(HttpContext http)
        {
            try
            {
                var cartId = http.Session.GetString("CartId");
                if (!string.IsNullOrWhiteSpace(cartId))
                {
                    return cartId;
                }

                return CreateNewCartCookie(http);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public string CreateNewCartCookie(HttpContext http)
        {
            try 
            {
                var newId = Guid.NewGuid().ToString("N");
                SetCartId(http, newId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void SetCartId(HttpContext http, string cartId)
        {   
            try
            {
                http.Session.SetString("CartId", cartId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Clear(HttpContext http)
        {
            try
            {
                http.Session.Remove("CartId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
