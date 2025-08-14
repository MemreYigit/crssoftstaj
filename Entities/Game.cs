using System.ComponentModel.DataAnnotations;
using static CrsSoft.Entities.Enums.EnumGameType;
using static CrsSoft.Entities.Enums.EnumPlatform;

namespace CrsSoft.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }                                 // Get or set game id 

        [Required]
        public required string Name { get; set; }                   // Get or set game name
        
        [Required]
        public required Platform Platform { get; set; }             // Get or set game's platform

        [Required]
        public required decimal Price { get; set; }                 // Get or set game's price

        public string? Description { get; set; }                    // Get or set game's description

        public GameType? Type { get; set; }                         // Get or set game's type 


        // For DataContext arrange relationship
        public List<Comment> Comments { get; set; } = new();
        public List<OrderItem> OrderItems { get; set; } = new();

    }
}
