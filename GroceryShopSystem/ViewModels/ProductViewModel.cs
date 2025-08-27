namespace GroceryShopSystem.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
    }
}
