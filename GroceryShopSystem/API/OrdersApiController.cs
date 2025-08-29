using GroceryShopSystem.Data;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.API
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersApiController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
