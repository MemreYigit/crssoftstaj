using System.Text.Json.Serialization;

namespace CrsSoft.Entities
{
    public class OrderItem
    {   
        // Order Relationship
        public int OrderID { get; set; }
        // Game Relationship
        public int GameID { get; set; }
        public int Quantity { get; set; } = 1;              // Get or set game quantity in order
        public decimal Price { get; set; }                  // Get or set price of game in order (quantity x game's price)

        [JsonIgnore]
        public Order Order { get; set; }
        public Game Game { get; set; }

    }
}
