using CrsSoft.Data;
using CrsSoft.Entities;
using CrsSoft.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CartService : ICartService
{
    private readonly DataContext dataContext;

    public CartService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<Cart> GetOrCreate(Guid cartId)
    {
        try
        {
            if (cartId == Guid.Empty)
            {
                cartId = Guid.NewGuid();
            }

            var cart = await dataContext.Carts.FindAsync(cartId);
            if (cart != null)
            {
                return cart;
            }
            
            var newCart = new Cart { Id = cartId };
            await dataContext.Carts.AddAsync(newCart);
            await dataContext.SaveChangesAsync();
            return newCart;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error get or create cart ıd: {ex.Message}");
        }
    }


    public async Task AddOrIncrementGame (Guid cartId, int gameId, int quantity = 1)
    {
        try
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            await EnsureCartExists(cartId);

            var item = await dataContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.GameId == gameId);

            if (item == null)
            {
                dataContext.CartItems.Add(new CartItem
                {
                    CartId = cartId,
                    GameId = gameId,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            await dataContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error increment item: {ex.Message}");
        }
    }


    public async Task DecrementGame(Guid cartId, int gameId, int quantity = 1)
    {
        try
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            await EnsureCartExists(cartId);

            var item = await dataContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.GameId == gameId);

            if (item == null)
            {
                return;
            }
            else if (item.Quantity > quantity)
            {
                item.Quantity -= quantity;
            }
            else
            {
                dataContext.CartItems.Remove(item);
            }
            
            await dataContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error decrement item: {ex.Message}");
        }
    }


    public async Task EmptyCart(Guid cartId)
    {
        try
        {
            var items = dataContext.CartItems
                .Where(ci => ci.CartId == cartId);
            dataContext.CartItems.RemoveRange(items);
            
            var cart = dataContext.Carts
                .FirstOrDefault(c => c.Id == cartId);
            if (cart != null)
            {
                dataContext.Carts.Remove(cart);
            }
            await dataContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error emptying cart: {ex.Message}");
        }
    }


    public async Task<object> GetCartDetails(Guid cartId)
    {
        try
        {
            var cart = await GetOrCreate(cartId);

            var items = await dataContext.CartItems
                .Where(i => i.CartId == cart.Id)
                .Select(i => new
                {
                    i.Id,
                    i.GameId,
                    GameName = i.Game.Name,     // from Games table
                    Price = i.Game.Price,    // from Games table
                    i.Quantity
                })
                .ToListAsync();

            return new 
            {
                CartId = cart.Id,
                Items = items,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting cart details: {ex.Message}");
        }
    }

    // Helper
    private async Task EnsureCartExists(Guid cartId)
    {
        try
        {
            if (!await dataContext.Carts.AnyAsync(c => c.Id == cartId))
            {
                dataContext.Carts.Add(new Cart { Id = cartId });
                await dataContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error ensuring cart exists: {ex.Message}");
        }
    }
}