using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;

namespace GroceryShopSystem.Services
{
	public class AuthApiServices
	{
		private readonly HttpClient _httpClient;

		const string API_BASE = "api/auth";

		public AuthApiServices(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your API base URL
		}

		// POST: api/auth/login
		public async Task<ApplicationUser?> LoginUserAsync(LoginViewModel loginViewModel)
		{
			var response = await _httpClient.PostAsJsonAsync($"{API_BASE}/login", loginViewModel);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<ApplicationUser>();
			}
			return null;
		}

		// POST: api/auth/logout
		public async Task<bool> LogoutUserAsync()
		{
			var response = await _httpClient.PostAsync($"{API_BASE}/logout", null);
			return response.IsSuccessStatusCode;
		}
	}
}
