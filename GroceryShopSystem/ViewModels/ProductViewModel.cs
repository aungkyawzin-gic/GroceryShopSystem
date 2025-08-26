namespace GroceryShopSystem.ViewModels
{
    public class ProductCreateViewModel
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public IFormFile? ImageFile { get; set; } 
        public bool IsActive { get; set; }
    }

}
