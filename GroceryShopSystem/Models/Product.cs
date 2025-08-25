using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }  

        // Foreign key for Category
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be >= 0")]
        public decimal Price { get; set; }
        
        public string ImageUrl { get; set; }    // pyan phyote yan    

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;        

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
