using GroceryShopSystem.Models;

namespace GroceryShopSystem.Services
{
    public class CartApiServices
    {
        private readonly HttpClient _httpClient;

        public CartApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/");
        }

        public async Task<IEnumerable<Cart>> GetCartAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Cart>>("api/CartApi");
        }

        public async Task<Cart?> GetProductAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Cart>($"api/cart/{id}");
        }
    }
}
