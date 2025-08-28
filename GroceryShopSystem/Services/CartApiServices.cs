using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using System.Net.Http.Json;

namespace GroceryShopSystem.Services
{
	public class CartApiServices
	{
		private readonly HttpClient _httpClient;

		const string API_BASE = "api/carts";
		public CartApiServices(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your API base URL
		}

		// GET: api/carts/{userId}
		public async Task<IEnumerable<CartItem>?> GetCartAsync(string userId)
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<CartItem>>($"{API_BASE}/{userId}");
		}

		// POST: api/carts/{userId} - Add item to cart
		public async Task<CartItem?> AddCartItemAsync(string userId, CartItemViewModel cartItem)
		{
			var response = await _httpClient.PostAsJsonAsync($"{API_BASE}/{userId}", cartItem);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<CartItem>();
			}
			return null;
		}

		// PATCH: api/carts/{userId}/{cartItemId} - Update cart item quantity
		public async Task<bool> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity)
		{
			var response = await _httpClient.PatchAsJsonAsync($"{API_BASE}/{userId}/{cartItemId}", quantity);
			return response.IsSuccessStatusCode;
		}

		// DELETE: api/carts/{userId}/{cartItemId} - Delete single cart item
		public async Task<bool> DeleteCartItemAsync(string userId, int cartItemId)
		{
			var response = await _httpClient.DeleteAsync($"{API_BASE}/{userId}/{cartItemId}");
			return response.IsSuccessStatusCode;
		}

		// DELETE: api/carts/{userId}/clear - Clear entire cart
		public async Task<bool> ClearCartAsync(string userId)
		{
			var response = await _httpClient.DeleteAsync($"{API_BASE}/{userId}/clear");
			return response.IsSuccessStatusCode;
		}

		// DELETE: api/carts/{userId}/cart - Delete the cart itself
		public async Task<bool> DeleteCartAsync(string userId)
		{
			var response = await _httpClient.DeleteAsync($"{API_BASE}/{userId}/cart");
			return response.IsSuccessStatusCode;
		}
	}
}
