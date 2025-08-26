using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }   // Django auto-generated id

        // One-to-One with Customer (User)
        [Required]
		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        // TODO
        // After create user, User add yan        

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;        
    }
}
