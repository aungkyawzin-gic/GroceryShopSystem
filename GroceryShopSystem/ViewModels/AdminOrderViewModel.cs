namespace GroceryShopSystem.ViewModels
{
    public class AdminOrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
