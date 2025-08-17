namespace CrsSoft.Interfaces
{
    public interface ICartCookieService
    {
        string GetOrCreate(HttpContext http);
        void Clear(HttpContext http);
    }
}
