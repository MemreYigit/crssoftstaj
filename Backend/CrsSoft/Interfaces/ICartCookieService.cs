namespace CrsSoft.Interfaces
{
    public interface ICartCookieService
    {
        string GetOrCreate(HttpContext http);
        string CreateNewCartCookie(HttpContext http); 
        void SetCartId(HttpContext http, string cartId); 
        void Clear(HttpContext http);
    }
}
