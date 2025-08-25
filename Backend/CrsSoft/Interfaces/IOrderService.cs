using CrsSoft.Entities;

namespace CrsSoft.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromCart(Guid cartId, int userId);
        Task<object> GetOrderDetails(int userId);
    }
}
