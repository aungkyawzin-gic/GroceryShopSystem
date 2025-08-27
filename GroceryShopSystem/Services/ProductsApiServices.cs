using GroceryShopSystem.Models;

namespace GroceryShopSystem.Services
{
    public class ProductsApiServices
    {
        private readonly HttpClient _httpClient;

        const string API_BASE = "api/productsApi";

        public ProductsApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your base URL
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{API_BASE}");
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"{API_BASE}/{id}");
        }

        // CREATE (POST)
        public async Task<Product?> CreateProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync($"{API_BASE}", product);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error {response.StatusCode}: {errorContent}");
            //return null;
        }

        // UPDATE (PUT)
        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"{API_BASE}/{id}", product);
            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{API_BASE}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
