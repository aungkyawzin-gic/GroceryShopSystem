using GroceryShopSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}/Details")]
        public async Task<IActionResult> Index(int id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
            return View(product);
        }
    }
}
