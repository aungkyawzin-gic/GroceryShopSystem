using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GroceryShopSystem.Controllers.Admin
{
    [Area("Admin")]
    [Route("Admin/Order")]
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
            IEnumerable<AdminOrderViewModel>? orders; 
            if (name != null)
            {
                orders = await _orderApiService.GetAllOrdersAsync();
            }
            else
            {
                orders = await _orderApiService.SearchOrdersByUsernameAsync(name);
            }

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
