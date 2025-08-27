using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for Cart
        [Required]
		[ForeignKey("Carts")]
		public int CartId { get; set; }
        public Cart Cart { get; set; }

        // Foreign key for Product
        [Required]
		[ForeignKey("Products")]
		public int ProductId { get; set; }
        public Product Product { get; set; }       

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
