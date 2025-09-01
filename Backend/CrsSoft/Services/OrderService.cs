// OrderService.cs
using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using static CrsSoft.Entities.Enums.EnumOrderStatus;

namespace CrsSoft.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext context;
        private readonly ICartService cartService;

        public OrderService(DataContext context, ICartService cartService)
        {
            this.context = context;
            this.cartService = cartService;
        }

        public async Task<Order> CreateOrderFromCart(Guid cartId, int userId)
        {
            try
            {
                var cart = await context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(ci => ci.Game)
                    .FirstOrDefaultAsync(c => c.Id == cartId && c.UserId == userId);

                if (cart == null || cart.Items.Count == 0)
                {
                    throw new InvalidOperationException("Cart is empty or does not exist.");
                }

                decimal totalPrice = cart.Items.Sum(item => item.Quantity * item.Game.Price);

                var order = new Order
                {
                    UserID = userId,
                    OrderPrice = totalPrice,
                    Status = OrderStatus.Created,
                    OrderDate = DateTime.UtcNow,
                    orderNumber = Guid.NewGuid(),
                    OrderItems = new List<OrderItem>()
                };

                foreach (var cartItem in cart.Items)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        GameID = cartItem.GameId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Quantity * cartItem.Game.Price
                    });
                }

                context.Orders.Add(order);
                await cartService.EmptyCart(cartId);
                await context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating order from cart: {ex.Message}");
            }
        }



        public async Task<object> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await context.Orders
                    .Where(o => o.UserID == userId)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Game)
                    .Include(o => o.User)
                    .Select(o => new
                    {
                        OrderId = o.Id,
                        OrderDate = o.OrderDate,
                        OrderPrice = o.OrderPrice,
                        Status = o.Status.ToString(),
                        OrderNumber = o.orderNumber,
                        OrderItems = o.OrderItems.Select(oi => new OrderItemGameName
                        {
                            GameID = oi.GameID,
                            GameName = oi.Game.Name,
                            GamePlatform = oi.Game.Platform,
                            Quantity = oi.Quantity,
                            Price = oi.Price
                        })
                    })
                    .OrderBy(o => o.OrderDate)
                    .ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting order details: {ex.Message}");
            }
        }


    }
}