using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using Newtonsoft.Json;

namespace GroceryShopSystem.Services
{
    public class CategoriesApiServices
    {
        private readonly HttpClient _httpClient;

        const string API_BASE = "api/categories";


        public CategoriesApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your base URL
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Category>>>($"{API_BASE}");
            return response.Data;
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Category>>($"{API_BASE}/{id}");
            return response?.Data;
        }

        // CREATE (POST)
        public async Task<Category?> CreateCategoryAsync(Category category)
        {
            var response = await _httpClient.PostAsJsonAsync($"{API_BASE}", category);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Category>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error {response.StatusCode}: {errorContent}");
            //return null;
        }

        // UPDATE (PUT)
        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            var response = await _httpClient.PutAsJsonAsync($"{API_BASE}/{id}", category);
            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{API_BASE}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
