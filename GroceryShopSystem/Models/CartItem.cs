using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for Cart
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        // Foreign key for Product
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }       

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
