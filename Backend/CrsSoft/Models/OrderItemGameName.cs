using static CrsSoft.Entities.Enums.EnumPlatform;

namespace CrsSoft.Models
{
    public class OrderItemGameName
    {
        public int GameID { get; set; }
        public string GameName { get; set; }
        public Platform GamePlatform { get; set; }            
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
