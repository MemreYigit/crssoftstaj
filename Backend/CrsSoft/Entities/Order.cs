using System.ComponentModel.DataAnnotations;
using static CrsSoft.Enums.EnumOrderStatus;

namespace CrsSoft.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }                                                   // Get or set order id 

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;                   // Get or set order created time

        [Required]
        public required decimal OrderPrice { get; set; }                              // Get or set order price

        [Required]
        public required OrderStatus Status { get; set; } = OrderStatus.Created;       // Get or set order status

        [Required]
        public required Guid orderNumber { get; set; } = Guid.NewGuid(); 

        // Relationship to User
        public int UserID { get; set; }
        public User User { get; set; }


        // For DataContext arrange relationship
        public List<OrderItem> OrderItems { get; set; } = new();

    }
}
