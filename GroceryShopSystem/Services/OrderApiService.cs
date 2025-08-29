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
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"{API_BASE }/admin");
        }

        // ADMIN: Get order by ID
        public async Task<AdminOrderViewModel?> GetOrderByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AdminOrderViewModel>($"{API_BASE}/admin/{id}");
        }

        // ADMIN: Search orders by username
        public async Task<List<AdminOrderViewModel>?> SearchOrdersByUsernameAsync(string? username)
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"{API_BASE}/admin/search/{username}");
        }

        // ADMIN: Search orders by userid
        public async Task<List<AdminOrderViewModel>?> SearchOrdersByUserIdAsync(string? userid)
        {
            return await _httpClient.GetFromJsonAsync<List<AdminOrderViewModel>>($"{API_BASE}/search/{userid}");
        }

        // ADMIN: Set order status to "delivered"
        public async Task<bool> SetOrderStatusToDeliveredAsync(int orderId)
        {
            var response = await _httpClient.PutAsync($"{API_BASE}/admin/{orderId}", null);
            return response.IsSuccessStatusCode;
        }

		// CUSTOMER: GET: api/orders/{userId} - Get user orders
		public async Task<List<Order>?> GetUserOrdersAsync(string userId)
		{
			return await _httpClient.GetFromJsonAsync<List<Order>>($"{API_BASE}/{userId}");
		}

		//CUSTOMER: POST: api/orders/{userId}/checkout - Proceed to checkout
		public async Task<Order?> ProceedToCheckoutAsync(string userId)
		{
			
			var response = await _httpClient.PostAsync($"{API_BASE}/{userId}/checkout", null);

			if (!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadFromJsonAsync<Order>();
		}

		//CUSTOMER: GET: api/orders/{userId}/{orderId}/orderitems - Get order items
		public async Task<List<OrderItem>?> GetOrderItemsAsync(string userId,int orderId)
		{
			return await _httpClient.GetFromJsonAsync<List<OrderItem>>($"{API_BASE}/{userId}/{orderId}/orderitems");
		}

		// CUSTOMER: POST: api/orders/{userId}/place - Place order
		public async Task<bool> PlaceOrderAsync(string userId, PlaceOrderViewModel request)
		{
			var response = await _httpClient.PostAsJsonAsync($"{API_BASE}/{userId}/place", request);

			return response.IsSuccessStatusCode;
		}

		// CUSTOMER: GET: api/orders/{userId}/details/{orderId} - Get order details
		public async Task<Order?> GetOrderDetailsAsync(string userId,int orderId)
		{
			return await _httpClient.GetFromJsonAsync<Order>($"{API_BASE}/{userId}/details/{orderId}");
		}

		// CUSTOMER: DELETE: api/orders/{userId}/{orderId} - Delete order
		public async Task<bool> DeleteOrderAsync(string userId,int orderId)
		{
			var response = await _httpClient.DeleteAsync($"{API_BASE}/{userId}/{orderId}");
			return response.IsSuccessStatusCode;
		}
	}
}
