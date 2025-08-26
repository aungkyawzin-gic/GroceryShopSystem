using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }   // Django auto-generated id

        [MaxLength(20)]
        public string OrderNo { get; set; }   // nullable by default

        
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // create, delivered
        

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal ShippingPrice { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Tax { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal GrandTotalPrice { get; set; } = 0.00m;

        public string Remark { get; set; }        
    }
}
