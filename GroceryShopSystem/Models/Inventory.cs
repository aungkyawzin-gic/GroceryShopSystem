using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; } 

        
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be >= 0")]
        public int Quantity { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int LowStockThreshold { get; set; } = 5;

        [Required]
        public bool IsVariant { get; set; } = false;

        // Reserved stock (for pending orders)
        public int Reserved { get; set; } = 0;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
       
    }
}
