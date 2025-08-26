using GroceryShopSystem.Models;

namespace GroceryShopSystem.Services
{
    public class ProductsApiServices
    {
        private readonly HttpClient _httpClient;

        public ProductsApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your base URL
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("api/productsApi");
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"api/productsApi/{id}");
        }
    }
}
