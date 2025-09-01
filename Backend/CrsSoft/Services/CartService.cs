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
        
    public async Task<Cart> GetOrCreateForUser(int userId)
    {
        try
        {
            var cart = await dataContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                return cart;
            }
            else 
            {
                var newCart = new Cart { Id = Guid.NewGuid(), UserId = userId };
                await dataContext.Carts.AddAsync(newCart);
                await dataContext.SaveChangesAsync();
                return newCart;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error get or create cart for user {userId}: {ex.Message}");
        }
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

    public async Task MergeCarts(Guid anonymousCartId, int userId)
    {
        using var transaction = await dataContext.Database.BeginTransactionAsync();

        try
        {
            var anonymousCart = await dataContext.Carts
                .AsTracking()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == anonymousCartId);

            if (anonymousCart == null || !anonymousCart.Items.Any())
            {
                await GetOrCreateForUser(userId); 
                await transaction.CommitAsync();
                return;
            }

            var userCart = await dataContext.Carts
                .AsTracking()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (userCart == null)
            {
                userCart = new Cart { Id = Guid.NewGuid(), UserId = userId };
                dataContext.Carts.Add(userCart);
                await dataContext.SaveChangesAsync();
            }

            foreach (var anonymousItem in anonymousCart.Items.ToList()) 
            {
                var existingItem = userCart.Items
                    .FirstOrDefault(i => i.GameId == anonymousItem.GameId);

                if (existingItem != null)
                {
                    await dataContext.CartItems
                        .Where(ci => ci.Id == existingItem.Id)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(ci => ci.Quantity, ci => ci.Quantity + anonymousItem.Quantity));
                }
                else
                {
                    dataContext.CartItems.Add(new CartItem
                    {
                        GameId = anonymousItem.GameId,
                        Quantity = anonymousItem.Quantity,
                        CartId = userCart.Id
                    });
                }
            }

            dataContext.CartItems.RemoveRange(anonymousCart.Items);
            dataContext.Carts.Remove(anonymousCart);

            await dataContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error merging carts: {ex.Message}");
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
            var items = dataContext.CartItems.Where(ci => ci.CartId == cartId);
            dataContext.CartItems.RemoveRange(items);
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
                    GameName = i.Game.Name,     
                    Price = i.Game.Price,   
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