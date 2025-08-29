using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GroceryShopSystem.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            IEnumerable<AdminOrderViewModel>? orders;


            if (name != null)
            {
                orders = await _orderApiService.SearchOrdersByUsernameAsync(name);
            }
            else
            {
                orders = await _orderApiService.GetAllOrdersAsync();
            }

            return View($"{AdminBase}Index.cshtml", orders);
        }

        [HttpGet("Users/{userid}")]
        public async Task<IActionResult> IndexByUserId(string? userid)
        {
            IEnumerable<AdminOrderViewModel>? orders = new List<AdminOrderViewModel>();


            if (userid != null)
            {
                orders = await _orderApiService.SearchOrdersByUserIdAsync(userid);
            }

            return View($"{AdminBase}IndexByUser.cshtml", orders);
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
        [HttpGet("{id}/Approve")]
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
    }
}
