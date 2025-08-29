using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;

namespace GroceryShopSystem.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;

        const string API_BASE = "api/orders/admin";

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your base URL
        }

        // ADMIN: Get all orders
        public async Task<List<AdminOrderViewModel>?> GetAllOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"{API_BASE}");
        }

        // ADMIN: Get order by ID
        public async Task<AdminOrderViewModel?> GetOrderByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AdminOrderViewModel>($"{API_BASE}/{id}");
        }

        // ADMIN: Search orders by username
        public async Task<List<AdminOrderViewModel>?> SearchOrdersByUsernameAsync(string? username)
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"{API_BASE}/search/{username}");
        }

        // ADMIN: Search orders by userid
        public async Task<List<AdminOrderViewModel>?> SearchOrdersByUserIdAsync(string? userid)
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"{API_BASE}/search/{userid}");
        }

        // ADMIN: Set order status to "delivered"
        public async Task<bool> SetOrderStatusToDeliveredAsync(int orderId)
        {
            var response = await _httpClient.PutAsync($"{API_BASE}/{orderId}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
