using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryId = p.CategoryId,
                    CategoryTitle = p.Category.Title
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryId = p.CategoryId,
                    CategoryTitle = p.Category.Title
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateViewModel model)
        {
            var category = await _context.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                return BadRequest(new { success = false, message = "Invalid category ID." });
            }

            var product = new Product
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                //IsActive = model.IsActive,
                Quantity = model.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = product });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
