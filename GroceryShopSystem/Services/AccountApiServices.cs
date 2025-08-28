using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;

namespace GroceryShopSystem.Services
{
	public class AccountApiServices
	{
		private readonly HttpClient _httpClient;

		const string API_BASE = "api/account";
		public AccountApiServices(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7074/"); // Your API base URL
		}

		// POST: api/account/register
		public async Task<ApplicationUser?> RegisterUserAsync(RegisterViewModel registerViewModel)
		{
			var response = await _httpClient.PostAsJsonAsync($"{API_BASE}/register", registerViewModel);
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<ApplicationUser>();
			}
			return null;
		}
	}
}
