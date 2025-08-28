using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;

namespace GroceryShopSystem.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;

        const string API_BASE = "api/orders";

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your base URL
        }

        // ADMIN: Get all orders
        public async Task<List<AdminOrderViewModel>?> GetAllOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>("api/orders/admin");
        }

        // ADMIN: Get order by ID
        public async Task<AdminOrderViewModel?> GetOrderByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AdminOrderViewModel>($"api/orders/admin/{id}");
        }

        // ADMIN: Search orders by username
        public async Task<List<AdminOrderViewModel>?> SearchOrdersByUsernameAsync(string username)
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"api/orders/admin/search/{username}");
        }

        // ADMIN: Set order status to "delivered"
        public async Task<bool> SetOrderStatusToDeliveredAsync(int orderId)
        {
            var response = await _httpClient.PutAsync($"api/orders/admin/{orderId}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
