using CrsSoft.Entities;

namespace CrsSoft.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateForUser(int userId);
        Task<Cart> GetOrCreate(Guid cartId);
        Task AddOrIncrementGame(Guid cartId, int gameId, int quantity = 1);
        Task DecrementGame(Guid cartId, int gameId, int quantity = 1);
        Task EmptyCart(Guid CartId);
        Task<object> GetCartDetails(Guid cartId);
        Task MergeCarts(Guid anonymousCartId, int userId); 
    }
}
