using System.ComponentModel.DataAnnotations;

namespace CrsSoft.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }                                 // Get or set user id
        
        [Required]
        public required string Name { get; set; }                   // Get or set user name
        
        public string? Surname { get; set; }                        // Get or set user surname
        
        [EmailAddress]
        [Required]
        public required string Email { get; set; }                  // Get or set user email

        [Required]
        public required string Password { get; set; }               // Get or set user password

        public decimal Money { get; set; } = 0.00m;                 // Get or set user money (wallet)



        // For DataContext arrange relationship
        public List<Comment> Comments { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}
