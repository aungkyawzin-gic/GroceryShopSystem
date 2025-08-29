using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(string sortOrder, int? categoryId)
        {
            ViewData["PriceSort"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "PriceDesc" ? "PriceAsc" : "PriceDesc";
            ViewData["AlphaSort"] = string.IsNullOrEmpty(sortOrder) || sortOrder == "AlphaDesc" ? "AlphaAsc" : "AlphaDesc";

            var products = _context.Products.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId.Value);

            products = sortOrder switch
            {
                "PriceAsc" => products.OrderBy(p => p.Price),
                "PriceDesc" => products.OrderByDescending(p => p.Price),
                "AlphaAsc" => products.OrderBy(p => p.Title),
                "AlphaDesc" => products.OrderByDescending(p => p.Title),
                _ => products.OrderBy(p => p.Id)
            };

            var categories = await _context.Categories.ToListAsync();
            ViewData["CategoryList"] = new SelectList(categories, "Id", "Title", categoryId);

            return View(await products.ToListAsync());

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
