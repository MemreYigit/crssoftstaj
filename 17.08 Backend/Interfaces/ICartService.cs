using CrsSoft.Entities;

namespace CrsSoft.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreate(Guid cartId);
        Task AddOrIncrementGame(Guid cartId, int gameId, int quantity = 1);
        Task DecrementGame(Guid cartId, int gameId, int quantity = 1);
        Task EmptyCart(Guid CartId);
        Task<object> GetCartDetails(Guid cartId);
    }
}
