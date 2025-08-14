using System.ComponentModel.DataAnnotations;

namespace CrsSoft.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }                                         // Get or set comment id       

        [Required]
        public required string CommentText { get; set; }                    // Get or set comment text

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;          // Get or set comment created time

        // Relationship to Game
        public int GameID { get; set; }
        public required Game Game { get; set; }

        // Relationship to User
        public int UserID { get; set; }
        public required User User { get; set; }
    }
}
