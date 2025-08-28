using GroceryShopSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;

namespace GroceryShopSystem.Controllers.Customer
{
    [Area("Customer")]
    [Route("Customer/Orders")]
    public class OrdersController : Controller
    {
        private readonly OrderApiService _orderService;

        public OrdersController(OrderApiService orderService)
        {
            _orderService = orderService;
        }

        // GET: Customer/Orders
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string userId = "4c776bb3-6d26-48eb-9a80-fe5fed95f9c2"; // Temporary UserId
            var orders = await _orderService.GetUserOrdersAsync(userId);

            if (orders == null)
            {
                TempData["Error"] = "No orders found.";
                return View("~/Views/Orders/Index.cshtml", new List<Order>());
            }

            return View("~/Views/Orders/Index.cshtml", orders);
        }

        // GET: Customer/Orders/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            string userId = "4c776bb3-6d26-48eb-9a80-fe5fed95f9c2"; // Temporary UserId
            var order = await _orderService.GetOrderDetailsAsync(userId, id);

            if (order == null)
            {
                return NotFound();
            }

            return View("~/Views/Orders/Details.cshtml", order);
        }

        // GET: Customer/Orders/PlaceOrder
        [HttpGet("PlaceOrder")]
        public IActionResult PlaceOrder()
        {
            return View("~/Views/Orders/PlaceOrder.cshtml");
        }

        // POST: Customer/Orders/PlaceOrder
        [HttpPost("PlaceOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(PlaceOrderViewModel request)
        {
            string userId = "4c776bb3-6d26-48eb-9a80-fe5fed95f9c2"; // Temporary UserId

            if (!ModelState.IsValid)
            {
                return View("~/Views/Orders/PlaceOrder.cshtml", request);
            }

            var result = await _orderService.PlaceOrderAsync(userId, request);
            if (result)
            {
                TempData["Success"] = "Order placed successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to place order.";
            return View("~/Views/Orders/PlaceOrder.cshtml", request);
        }

        // DELETE: Customer/Orders/Delete/{id}
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = "4c776bb3-6d26-48eb-9a80-fe5fed95f9c2"; // Temporary UserId
            var result = await _orderService.DeleteOrderAsync(userId, id);

            if (!result)
            {
                TempData["Error"] = "Failed to delete order.";
            }
            else
            {
                TempData["Success"] = "Order deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
