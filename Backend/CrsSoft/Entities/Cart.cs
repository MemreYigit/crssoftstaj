using System.ComponentModel.DataAnnotations;

namespace CrsSoft.Entities
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }
}
