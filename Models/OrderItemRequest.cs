namespace CrsSoft.Models
{
    public class OrderItemRequest
    {
        public int OrderID { get; set; }
        public int GameID { get; set; }
        public int Quantity { get; set; }
    }
}
