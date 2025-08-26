using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }  

        // Foreign key for Order
        [Required]
		[ForeignKey("Orders")]
		public int OrderId { get; set; }
        public Order Order { get; set; }
       
        [Required]
		[ForeignKey("Products")]
		public int ProductId { get; set; }
       
        public Product Product { get; set; }       

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceAtPurchase { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;       

    }
}
