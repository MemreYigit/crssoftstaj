using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CrsSoft.Entities
{
    public class CartItem
    {   
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int GameId { get; set; }
        public Guid CartId { get; set; }

        [JsonIgnore]
        public Game Game { get; set; }
        public Cart Cart { get; set; }
    }
}
