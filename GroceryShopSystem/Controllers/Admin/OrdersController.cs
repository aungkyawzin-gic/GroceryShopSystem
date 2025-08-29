using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GroceryShopSystem.Controllers.Admin
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    [Route("Admin/Orders")]
    public class OrdersController : Controller
    {
        const string AdminBase = "~/Views/Admin/Order/";

        private readonly OrderApiService _orderApiService;

        public OrdersController(OrderApiService orderApiService)
        {
            _orderApiService = orderApiService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? name)
        {
            //IEnumerable<AdminOrderViewModel>? orders; 

            var orders = new List<AdminOrderViewModel>
            {
                new AdminOrderViewModel
            {
                OrderId = 1,
                OrderNo = "ORD001",
                UserName = "user1",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                TotalPrice = 50.00m,
                ShippingPrice = 5.00m,
                Tax = 2.00m,
                GrandTotal = 57.00m,
                Status = "create",
                Remark = "First test order",
                Items = new List<OrderItemViewModel>
                {
                    new OrderItemViewModel { ProductName = "Apple", Quantity = 2, PriceAtPurchase = 20.00m },
                    new OrderItemViewModel { ProductName = "Banana", Quantity = 3, PriceAtPurchase = 10.00m }
                }
            },
            new AdminOrderViewModel
            {
                OrderId = 2,
                OrderNo = "ORD002",
                UserName = "user1",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                TotalPrice = 120.00m,
                ShippingPrice = 8.00m,
                Tax = 6.00m,
                GrandTotal = 134.00m,
                Status = "delivered",
                Remark = "Delivered successfully",
                Items = new List<OrderItemViewModel>
                {
                    new OrderItemViewModel { ProductName = "Orange", Quantity = 5, PriceAtPurchase = 50.00m },
                    new OrderItemViewModel { ProductName = "Grapes", Quantity = 4, PriceAtPurchase = 70.00m }
                }
            },
            new AdminOrderViewModel
            {
                OrderId = 3,
                OrderNo = "ORD003",
                UserName = "user1",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                TotalPrice = 75.00m,
                ShippingPrice = 5.00m,
                Tax = 3.00m,
                GrandTotal = 83.00m,
                Status = "delivered",
                Remark = "Waiting for shipment",
                Items = new List<OrderItemViewModel>
                {
                    new OrderItemViewModel { ProductName = "Mango", Quantity = 3, PriceAtPurchase = 45.00m },
                    new OrderItemViewModel { ProductName = "Pineapple", Quantity = 1, PriceAtPurchase = 30.00m }
                }
            }
            };
            //if (name != null)
            //{
            //    orders = await _orderApiService.GetAllOrdersAsync();
            //}
            //else
            //{
            //    orders = await _orderApiService.SearchOrdersByUsernameAsync(name);
            //}

            return View($"{AdminBase}Index.cshtml", orders);
        }
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderApiService.GetOrderByIdAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View($"{AdminBase}Details.cshtml", order);
        }
        
        // POST action to receive user input address
        [HttpPost("{id}/Approve")]
        public async Task<IActionResult> ApproveOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = await _orderApiService.SetOrderStatusToDeliveredAsync(id.Value);

            if (res)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        //[HttpPost("")]
        //public async Task<IActionResult> ApproveOrder(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var res = await _orderApiService.SetOrderStatusToDeliveredAsync(id.Value);

        //    if (res)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View("~/Views/Shared/Error.cshtml");
        //}
    }
}
