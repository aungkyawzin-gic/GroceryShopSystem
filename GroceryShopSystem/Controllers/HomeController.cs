using System.Diagnostics;
using System.Runtime.CompilerServices;
using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
                ViewData["Layout"] = "_LayoutAdmin";
            else if (User.IsInRole("User"))
                ViewData["Layout"] = "_LayoutUser";
            return View(await _context.Product.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
