using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using System.Net.Http.Json;

namespace GroceryShopSystem.Services
{
    public class UserApiServices
    {
        private readonly HttpClient _httpClient;
        private const string API_BASE = "api/users";

        public UserApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/");
        }

        // GET ALL USERS
        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ApplicationUser>>>($"{API_BASE}");
            return response?.Data ?? new List<ApplicationUser>();
        }

        // GET USER BY ID
        public async Task<ApplicationUser?> GetUserAsync(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ApplicationUser>>($"{API_BASE}/{id}");
            return response?.Data;
        }

        // CREATE USER
        public async Task<ApplicationUser?> CreateUserAsync(ApplicationUser user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{API_BASE}", user);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApplicationUser>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error {response.StatusCode}: {errorContent}");
        }

        // UPDATE USER
        public async Task<bool> UpdateUserAsync(string id, ApplicationUser user)
        {
            var response = await _httpClient.PutAsJsonAsync($"{API_BASE}/{id}", user);
            return response.IsSuccessStatusCode;
        }

        // DELETE USER
        public async Task<bool> DeleteUserAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{API_BASE}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
