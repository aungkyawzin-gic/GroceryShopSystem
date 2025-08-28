using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.API
{
	[Route("api/carts")]
	[ApiController]
	public class CartsApiController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CartsApiController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/carts/{userId}
		[HttpGet("{userId}")]
		public async Task<ActionResult<IEnumerable<CartItem>>> GetCart(string userId)
		{
			var cartItems = await _context.CartItems
							 .Include(ci => ci.Product)
							 .Include(ci => ci.Cart)
							 .Where(ci => ci.Cart.UserId == userId)
							 .ToListAsync();

			if (cartItems == null || cartItems.Count == 0)
			{
				return NotFound(new { message = "No cart items found for this user." });
			}

			return Ok(cartItems);
		}

		// POST: api/carts/{userId}
		[HttpPost("{userId}")]
		public async Task<ActionResult<CartItem>> CreateCartItem(string userId, CartItem cartItemRequest)
		{
			// Find the user's cart
			var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				// If the user doesn't have a cart yet, create one
				cart = new Cart
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};
				_context.Carts.Add(cart);
				await _context.SaveChangesAsync();
			}

			// Check if the product exists in the database
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartItemRequest.ProductId);
			if (product == null)
				return NotFound(new { message = "Product not found." });

			// Check if the product is already in the cart
			var existingCartItem = await _context.CartItems
				.FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == cartItemRequest.ProductId);

			int newQuantity = cartItemRequest.Quantity;
			if (existingCartItem != null)
				newQuantity += existingCartItem.Quantity;

			// Check if requested quantity exceeds available stock
			if (newQuantity > product.Quantity)
				return BadRequest(new { message = $"Cannot add {cartItemRequest.Quantity} items. Only {product.Quantity - (existingCartItem?.Quantity ?? 0)} left in stock." });

			if (existingCartItem != null)
			{
				// Update quantity if already exists
				existingCartItem.Quantity = newQuantity;
				existingCartItem.UpdatedAt = DateTime.UtcNow;
				_context.CartItems.Update(existingCartItem);
			}
			else
			{
				// Add new cart item
				var newCartItem = new CartItem
				{
					CartId = cart.Id,
					ProductId = cartItemRequest.ProductId,
					Quantity = cartItemRequest.Quantity,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};
				_context.CartItems.Add(newCartItem);
			}

			await _context.SaveChangesAsync();

			return Ok(new { message = "Item added to cart successfully." });
		}

		// PATCH: api/carts/{userId}/{cartItemId}
		[HttpPatch("{userId}/{cartItemId}")]
		public async Task<IActionResult> UpdateCartItemQuantity(string userId, int cartItemId, [FromBody] int quantity)
		{
			if (quantity <= 0)
				return BadRequest(new { message = "Quantity must be greater than zero." });
			// Find the cart item for the user
			var cartItem = await _context.CartItems
										 .Include(ci => ci.Product)
										 .Include(ci => ci.Cart)
										 .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);
			if (cartItem == null)
				return NotFound(new { message = "Cart item not found." });
			// Check if requested quantity exceeds available stock
			if (quantity > cartItem.Product.Quantity)
				return BadRequest(new { message = $"Cannot set quantity to {quantity}. Only {cartItem.Product.Quantity} left in stock." });
			// Update the quantity
			cartItem.Quantity = quantity;
			cartItem.UpdatedAt = DateTime.UtcNow;
			_context.CartItems.Update(cartItem);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Cart item quantity updated successfully." });
		}

		// DELETE: api/carts/{userId}/{cartItemId}
		[HttpDelete("{userId}/{cartItemId}")]
		public async Task<IActionResult> DeleteCartItem(string userId, int cartItemId)
		{
			// Find the cart item for the user
			var cartItem = await _context.CartItems
										 .Include(ci => ci.Cart)
										 .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);
			if (cartItem == null)
				return NotFound(new { message = "Cart item not found." });
			// Remove the cart item
			_context.CartItems.Remove(cartItem);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Cart item deleted successfully." });
		}

		//DELETE: api/carts/{userId}/clear
		[HttpDelete("{userId}/clear")]
		public async Task<IActionResult> ClearCart(string userId)
		{
			// Find all cart items for the user
			var cartItems = await _context.CartItems
										 .Include(ci => ci.Cart)
										 .Where(ci => ci.Cart.UserId == userId)
										 .ToListAsync();
			if (cartItems == null || cartItems.Count == 0)
				return NotFound(new { message = "No cart items." });

			// Remove all cart items
			_context.CartItems.RemoveRange(cartItems);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Cart cleared successfully." });
		}

		// DELETE: api/carts/{userId}/{cardtId}
		[HttpDelete("{userId}/cart")]
		public async Task<IActionResult> DeleteCart(string userId)
		{
			// Find the user's cart
			var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
			if (cart == null)
				return NotFound(new { message = "Cart not found." });
			// Remove all cart items associated with the cart
			var cartItems = await _context.CartItems.Where(ci => ci.CartId == cart.Id).ToListAsync();
			_context.CartItems.RemoveRange(cartItems);
			// Remove the cart itself
			_context.Carts.Remove(cart);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Cart deleted successfully." });
		}
	}
}
