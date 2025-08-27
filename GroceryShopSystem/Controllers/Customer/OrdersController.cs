using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Customer
{
    [Area("Customer")]
    [Route("Order")]
    public class OrdersController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            // Mock orders data
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderNo = "ORD001",
                    UserId = "user1",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow,
                    Status = "create",
                    TotalPrice = 50.00m,
                    ShippingPrice = 5.00m,
                    Tax = 2.00m,
                    GrandTotalPrice = 57.00m,
                    Remark = "First test order",
                    OrderItems = new List<OrderItem>()
                },
                new Order
                {
                    Id = 2,
                    OrderNo = "ORD002",
                    UserId = "user1",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow,
                    Status = "delivered",
                    TotalPrice = 120.00m,
                    ShippingPrice = 8.00m,
                    Tax = 6.00m,
                    GrandTotalPrice = 134.00m,
                    Remark = "Delivered successfully",
                    OrderItems = new List<OrderItem>()
                },
                new Order
                {
                    Id = 3,
                    OrderNo = "ORD003",
                    UserId = "user1",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow,
                    Status = "pending",
                    TotalPrice = 75.00m,
                    ShippingPrice = 5.00m,
                    Tax = 3.00m,
                    GrandTotalPrice = 83.00m,
                    Remark = "Waiting for shipment",
                    OrderItems = new List<OrderItem>()
                }
            };

            return View("~/Views/Orders/Index.cshtml", orders);
        }
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            // Mock order with items
            var order = new Order
            {
                Id = id,
                OrderNo = $"ORD00{id}",
                UserId = "user1",
                CreatedAt = DateTime.UtcNow.AddDays(-id),
                UpdatedAt = DateTime.UtcNow,
                Status = id % 2 == 0 ? "delivered" : "pending",
                TotalPrice = 120,
                ShippingPrice = 8,
                Tax = 6,
                GrandTotalPrice = 134,
                Remark = "Sample order details",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = 1, ProductId = 101, Quantity = 2, PriceAtPurchase = 20, Product = new Product { Title = "Fresh Apples" } },
                    new OrderItem { Id = 2, ProductId = 102, Quantity = 1, PriceAtPurchase = 50, Product = new Product { Title = "Organic Milk" } },
                    new OrderItem { Id = 3, ProductId = 103, Quantity = 3, PriceAtPurchase = 10, Product = new Product { Title = "Whole Wheat Bread" } }
                }
            };

            return View("~/Views/Orders/Details.cshtml", order);
        }
        // GET: /Order/OrderForm
        [HttpGet("OrderForm")]
        public IActionResult OrderForm()
        {
            // Mock cart items
            var cartItems = new List<CartItem>
            {
                new CartItem { Id = 1, Quantity = 2, Product = new Product { Title = "Fresh Apples", Price = 20 } },
                new CartItem { Id = 2, Quantity = 1, Product = new Product { Title = "Organic Milk", Price = 50 } },
                new CartItem { Id = 3, Quantity = 3, Product = new Product { Title = "Whole Wheat Bread", Price = 10 } }
            };
            return View($"~/Views/Orders/OrderForm.cshtml", cartItems);
        }

        // POST action to receive user input address
        [HttpPost("ConfirmOrder")]
        public IActionResult ConfirmOrder(Address shippingAddress)
        {
            // shippingAddress contains user input
            TempData["Message"] = $"Order confirmed! Shipping to: {shippingAddress.Street}, {shippingAddress.City}, {shippingAddress.State}";
            return RedirectToAction("Index", "Orders", new { area = "Customer" });
        }
    }
}
