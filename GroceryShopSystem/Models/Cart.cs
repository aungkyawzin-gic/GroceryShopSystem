using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }   // Django auto-generated id

        // One-to-One with Customer (User)
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        // TODO
        // After create user, User add yan        

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    }
}