using CrsSoft.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrsSoft.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        // Cart
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            // 1 Game -> Many Comments
            modelBuilder.Entity<Comment>()
                .HasOne(g => g.Game)
                .WithMany(c => c.Comments)
                .HasForeignKey(g => g.GameID);

            // 1 User -> Many Comments 
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(u => u.UserID);

            // 1 User -> Many Orders
            modelBuilder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(u => u.UserID);
                       
            // 1 Order -> Many OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(o => o.OrderID);

            // 1 Game -> Many OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne(g => g.Game)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(g => g.GameID);

            // Primary Key of OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderID, oi.GameID });

            // For Decimal Values
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderPrice)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<User>()
                .Property(u => u.Money)
                .HasColumnType("decimal(10,2)");

            // Cart and CartItem
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(i => i.Game)
                .WithMany()
                .HasForeignKey(i => i.GameId);
        }
    }
}
