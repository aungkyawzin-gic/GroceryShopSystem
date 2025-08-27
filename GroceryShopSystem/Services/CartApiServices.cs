using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using System.Net.Http.Json;

namespace GroceryShopSystem.Services
{
	public class CartApiServices
	{
		private readonly HttpClient _httpClient;

		public CartApiServices(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your API base URL
		}

		// GET: api/Carts/{userId}
		public async Task<IEnumerable<CartItem>?> GetCartAsync(string userId)
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<CartItem>>($"api/CartsApi/{userId}");
		}

		// POST: api/Carts/{userId} - Add item to cart
		public async Task<CartItem?> AddCartItemAsync(string userId, CartItemViewModel cartItem)
		{
			var response = await _httpClient.PostAsJsonAsync($"api/CartsApi/{userId}", cartItem);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<CartItem>();
			}
			return null;
		}

		// PATCH: api/Carts/{userId}/product/{productId} - Update cart item quantity
		public async Task<bool> UpdateCartItemQuantityAsync(string userId, int productId, int quantity)
		{
			var response = await _httpClient.PatchAsJsonAsync($"api/CartsApi/user/{userId}/product/{productId}", quantity);
			return response.IsSuccessStatusCode;
		}

		// DELETE: api/Carts/{userId}/product/{productId} - Delete single cart item
		public async Task<bool> DeleteCartItemAsync(string userId, int productId)
		{
			var response = await _httpClient.DeleteAsync($"api/CartsApi/user/{userId}/product/{productId}");
			return response.IsSuccessStatusCode;
		}

		// DELETE: api/Carts/{userId}/clear - Clear entire cart
		public async Task<bool> ClearCartAsync(string userId)
		{
			var response = await _httpClient.DeleteAsync($"api/CartsApi/user/{userId}/clear");
			return response.IsSuccessStatusCode;
		}

		// DELETE: api/Carts/{userId}/cart/{cartId} - Delete the cart itself
		public async Task<bool> DeleteCartAsync(string userId)
		{
			var response = await _httpClient.DeleteAsync($"api/CartsApi/user/{userId}/cart");
			return response.IsSuccessStatusCode;
		}
	}
}
