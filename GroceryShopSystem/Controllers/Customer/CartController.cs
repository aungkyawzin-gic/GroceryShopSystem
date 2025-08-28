using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryShopSystem.Controllers.Customer
{
    [Area("Customer")]
    [Route("Customer/[controller]")]
    public class CartController : Controller
    {
        private readonly CartApiServices _services;

        public CartController(CartApiServices services)
        {
            _services = services;
        }

        // GET: /Cart/Index
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string userId = "f390c3c1-5f1e-42c0-8050-dcc4f06d1ec1"; // Replace with logged-in user's Id
            IEnumerable<CartItem>? cartItems = null;

            try
            {
                cartItems = await _services.GetCartAsync(userId);
            }
            catch
            {
                // fallback: empty cart
                cartItems = new List<CartItem>();
            }

            return View("~/Views/Cart/Index.cshtml", cartItems);
        }

        // Optional: Add to cart example
        [HttpPost("Add")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            string userId = "f390c3c1-5f1e-42c0-8050-dcc4f06d1ec1";
            var newItem = new CartItemViewModel { ProductId = productId, Quantity = quantity };

            var addedItem = await _services.AddCartItemAsync(userId, newItem);
            if (addedItem != null)
                return Json(new { success = true, message = "Item added to cart!" });
            else
                return Json(new { success = false, message = "Failed to add item." });
        }

        // Optional: Update quantity
        [HttpPost("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            string userId = "f390c3c1-5f1e-42c0-8050-dcc4f06d1ec1"; // Replace with logged-in user's Id

            // Find productId for cartItemId if needed
            // For simplicity, assume cartItemId = productId in this mock
            bool success = await _services.UpdateCartItemQuantityAsync(userId, cartItemId, quantity);

            if (!success) TempData["Error"] = "Failed to update quantity.";
            return RedirectToAction("Index");
        }

        // Optional: Remove item
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            string userId = "f390c3c1-5f1e-42c0-8050-dcc4f06d1ec1"; // Replace with logged-in user's Id
            bool success = await _services.DeleteCartItemAsync(userId, cartItemId);

            if (!success) TempData["Error"] = "Failed to remove item.";
            return RedirectToAction("Index");
        }
    }
}
