namespace CrsSoft.Models
{
    public class OrderRequest
    {
        public int UserID { get; set; }
        public decimal OrderPrice { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; } = new(); // değişicek
    }
}
