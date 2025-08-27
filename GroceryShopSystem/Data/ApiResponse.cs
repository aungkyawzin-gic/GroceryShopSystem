namespace GroceryShopSystem.Data
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int Count { get; set; }
        public T Data { get; set; }
    }
}
