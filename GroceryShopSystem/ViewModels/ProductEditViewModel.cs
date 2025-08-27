
namespace GroceryShopSystem.ViewModels
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

}
