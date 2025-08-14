using System.ComponentModel.DataAnnotations;
using static CrsSoft.Entities.Enums.EnumOrderStatus;

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


        // Relationship to User
        public int UserID { get; set; }
        public required User User { get; set; }


        // For DataContext arrange relationship
        public List<OrderItem> OrderItems { get; set; } = new();

    }
}
