using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.API
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                count = categories.Count,
                data = categories
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (category == null)
                return NotFound(new { success = false, message = "Category not found." });

            return Ok(new { success = true, data = category });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateViewModel model)
        {
            if (_context.Categories.Any(c => c.Title == model.Title))
            {
                return BadRequest(new { success = false, message = "This category name already exists." });
            }

            var category = new Category
            {
                Title = model.Title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new
            {
                success = true,
                data = category
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest(new { success = false, message = "ID mismatch." });

            if (_context.Categories.Any(c => c.Title == category.Title && c.Id != id))
            {
                return BadRequest(new { success = false, message = "This category name already exists." });
            }

            try
            {
                category.UpdatedAt = DateTime.UtcNow;
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id))
                    return NotFound(new { success = false, message = "Category not found." });
                else
                    throw;
            }

            return Ok(new { success = true, message = "Category updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { success = false, message = "Category not found." });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Category deleted successfully." });
        }
    }
}
