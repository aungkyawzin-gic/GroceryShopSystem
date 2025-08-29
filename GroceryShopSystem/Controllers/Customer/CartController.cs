using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Customer
{
    [Area("Customer")]
    [Route("Customer/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly CartApiServices _services;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(CartApiServices services, UserManager<ApplicationUser> userManager)
        {
            _services = services;
            _userManager = userManager;
        }

        // GET: /Cart/Index
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<CartItem>? cartItems = null;

            try
            {
                cartItems = await _services.GetCartAsync(userId);
            }
            catch
            {
                cartItems = new List<CartItem>();
            }

            return View("~/Views/Cart/Index.cshtml", cartItems);
        }

        // Optional: Add to cart example
        [HttpPost("Add")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            string userId = _userManager.GetUserId(User);
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
            string userId = _userManager.GetUserId(User); // Replace with logged-in user's Id

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
            string userId = _userManager.GetUserId(User); // Replace with logged-in user's Id
            bool success = await _services.DeleteCartItemAsync(userId, cartItemId);

            if (!success) TempData["Error"] = "Failed to remove item.";
            return RedirectToAction("Index");
        }
    }
}
