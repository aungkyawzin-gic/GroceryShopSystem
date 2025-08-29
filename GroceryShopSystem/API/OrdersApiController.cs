using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GroceryShopSystem.API
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly OrderSettings _orderSettings;

		public OrdersApiController(ApplicationDbContext context, IOptions<OrderSettings> orderSettings)
		{
			_context = context;
			_orderSettings = orderSettings.Value;
		}

		// GET: api/orders/admin
		[HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<AdminOrderViewModel>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var result = orders.Select(o => new AdminOrderViewModel
            {
                OrderId = o.Id,
                OrderNo = o.OrderNo,
                UserName = o.User.FullName,
                CreatedAt = o.CreatedAt,
                TotalPrice = o.TotalPrice,
                ShippingPrice = o.ShippingPrice,
                Tax = o.Tax,
                GrandTotal = o.GrandTotalPrice,
                Status = o.Status,
                Remark = o.Remark,
                Items = o.OrderItems.Select(i => new OrderItemViewModel
                {
                    ProductName = i.Product.Title,
                    Quantity = i.Quantity,
                    PriceAtPurchase = i.PriceAtPurchase
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/orders/admin/{id}
        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<AdminOrderViewModel>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new AdminOrderViewModel
            {
                OrderId = order.Id,
                OrderNo = order.OrderNo,
                UserName = order.User.FullName,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                ShippingPrice = order.ShippingPrice,
                Tax = order.Tax,
                GrandTotal = order.GrandTotalPrice,
                Status = order.Status,
                Remark = order.Remark,
                Items = order.OrderItems.Select(i => new OrderItemViewModel
                {
                    ProductName = i.Product.Title,
                    Quantity = i.Quantity,
                    PriceAtPurchase = i.PriceAtPurchase
                }).ToList()
            };

            return Ok(viewModel);
        }

        // GET: api/orders/admin/search/{username}
        [HttpGet("admin/search/{username}")]
        public async Task<ActionResult<IEnumerable<AdminOrderViewModel>>> GetOrdersByUserName(string username)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.User.FullName.ToLower().Contains(username.ToLower()))
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound($"No orders found for user name containing: {username}");
            }

            var result = orders.Select(o => new AdminOrderViewModel
            {
                OrderId = o.Id,
                OrderNo = o.OrderNo,
                UserName = o.User.FullName,
                CreatedAt = o.CreatedAt,
                TotalPrice = o.TotalPrice,
                ShippingPrice = o.ShippingPrice,
                Tax = o.Tax,
                GrandTotal = o.GrandTotalPrice,
                Status = o.Status,
                Remark = o.Remark,
                Items = o.OrderItems.Select(i => new OrderItemViewModel
                {
                    ProductName = i.Product.Title,
                    Quantity = i.Quantity,
                    PriceAtPurchase = i.PriceAtPurchase
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/orders/admin/search/{username}
        [HttpGet("admin/search/{userid}")]
        public async Task<ActionResult<IEnumerable<AdminOrderViewModel>>> GetOrdersByUserId(string userid)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.User.Id == userid)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound($"No orders found for user id with: {userid}");
            }

            var result = orders.Select(o => new AdminOrderViewModel
            {
                OrderId = o.Id,
                OrderNo = o.OrderNo,
                UserName = o.User.FullName,
                CreatedAt = o.CreatedAt,
                TotalPrice = o.TotalPrice,
                ShippingPrice = o.ShippingPrice,
                Tax = o.Tax,
                GrandTotal = o.GrandTotalPrice,
                Status = o.Status,
                Remark = o.Remark,
                Items = o.OrderItems.Select(i => new OrderItemViewModel
                {
                    ProductName = i.Product.Title,
                    Quantity = i.Quantity,
                    PriceAtPurchase = i.PriceAtPurchase
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // PUT: api/orders/admin/deliver/{id}
        [HttpPut("admin/deliver/{id:int}")]
        public async Task<IActionResult> SetOrderStatusToDelivered(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound(new { message = $"Order with ID {id} not found." });
            }

            order.Status = "delivered";
            order.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Order {order.OrderNo} marked as delivered." });
        }

        // GET: api/orders/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

		// POST: api/orders/{userId}/checkout
		[HttpPost("{userId}/checkout")]
		public async Task<IActionResult> ProceedToCheckout(string userId)
		{
			// Find user
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
				return NotFound("User not found.");

			// Find user's cart
			var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
			if (cart == null)
				return NotFound("Your cart is not found.");

			var cartItems = await _context.CartItems
				.Include(ci => ci.Product)
				.Where(ci => ci.CartId == cart.Id)
				.ToListAsync();

			if (cartItems.Count == 0)
				return BadRequest("Your cart is empty.");

			// Create an order (status = pending)
			var order = new Order
			{
				OrderNo = "ORD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
				UserId = userId,
				Status = "pending",
				TotalPrice = 0,
				ShippingPrice = 0,
				Tax = 0,
				GrandTotalPrice = 0,
                Remark = "Default"
			};

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			// Move cart items to order items
			foreach (var item in cartItems)
			{
				var orderItem = new OrderItem
				{
					OrderId = order.Id,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					PriceAtPurchase = item.Product.Price,
				};
				_context.OrderItems.Add(orderItem);
			}

			// Clear cart
			_context.CartItems.RemoveRange(cartItems);
			_context.Carts.Remove(cart);

			await _context.SaveChangesAsync();

			return Ok(new
			{
				order.Id,
				order.OrderNo,
				order.Status,
				Message = "Successfully proceed checkout."
			});
		}

		// POST: api/orders/{userId}/place
		[HttpPost("{userId}/place")]
		public async Task<IActionResult> PlaceOrder(string userId, PlaceOrderViewModel placeOrderViewModel)
		{
			// Find user
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
				return NotFound("User not found.");

			// Find the most recent pending order for this customer
			var order = await _context.Orders
				.Where(o => o.UserId == userId && o.Status == "pending")
				.OrderByDescending(o => o.CreatedAt)
				.FirstOrDefaultAsync();

			if (order == null)
				return NotFound("No pending order found. Please proceed to checkout first.");

			// Handle address
			if (user.AddressId.HasValue)
			{
				var existingAddress = await _context.Addresses.FindAsync(user.AddressId.Value);
				if (existingAddress != null)
				{
					existingAddress.Street = placeOrderViewModel.Street;
					existingAddress.City = placeOrderViewModel.City;
					existingAddress.State = placeOrderViewModel.State;
					_context.Addresses.Update(existingAddress);
				}
			}
			else
			{
				var newAddress = new Address
				{
					Street = placeOrderViewModel.Street,
					City = placeOrderViewModel.City,
					State = placeOrderViewModel.State
				};
				_context.Addresses.Add(newAddress);
				await _context.SaveChangesAsync();

				user.AddressId = newAddress.Id;
				_context.Users.Update(user);
			}

			await _context.SaveChangesAsync();

			// Calculate totals using order items
			var orderItems = await _context.OrderItems
				.Include(oi => oi.Product)
				.Where(oi => oi.OrderId == order.Id)
				.ToListAsync();

			decimal shipping = _orderSettings.ShippingFee;
			decimal tax = placeOrderViewModel.TotalPrice * _orderSettings.TaxRate;
			decimal grandTotal = placeOrderViewModel.TotalPrice + shipping + tax;

			// Update order
			order.TotalPrice = placeOrderViewModel.TotalPrice;
			order.ShippingPrice = shipping;
			order.Tax = tax;
			order.GrandTotalPrice = grandTotal;
			order.Status = "created";

			_context.Orders.Update(order);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				order.Id,
				order.OrderNo,
				order.GrandTotalPrice,
				order.Status,
				Message = "Order placed successfully."
			});
		}

		// GET: api/orders/{userId}/details/{orderId}
		[HttpGet("{userId}/details/{orderId}")]

        public async Task<IActionResult> GetOrderDetails(string userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null)
            {
                return NotFound(new { message = $"Order with ID {orderId} not found for this user." });
            }
            return Ok(order);
        }

        //DELETE: api/orders/{userId}/{orderId}
        [HttpDelete("{userId}/{orderId}")]
        public async Task<IActionResult> DeleteOrder(string userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
				.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null)
            {
                return NotFound(new { message = $"Order with ID {orderId} not found for this user." });
            }
            _context.OrderItems.RemoveRange(order.OrderItems);
			_context.Orders.Remove(order);
            await _context.SaveChangesAsync();
			return Ok(new { message = $"Order {order.OrderNo} and its items have been deleted." });
		}
    }
}