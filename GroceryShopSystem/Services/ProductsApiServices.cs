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

        // CREATE (POST)
        public async Task<Product?> CreateProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/productsApi", product);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            return null;
        }

        // UPDATE (PUT)
        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/productsApi/{id}", product);
            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/productsApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
