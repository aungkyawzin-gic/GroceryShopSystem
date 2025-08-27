using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Controllers.Admin
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class CategoriesController : Controller
    {
        private readonly CategoriesApiServices _services;
        private readonly ApplicationDbContext _context;

        const string AdminBase = "~/Views/Admin/Category/";

        public CategoriesController(CategoriesApiServices services, ApplicationDbContext context)
        {
            _services = services;
            _context = context;
        }


        [HttpGet("")]
        // GET: CategoriesView
        public async Task<IActionResult> Index()
        {
            var Categories = await _services.GetCategoriesAsync();
            return View($"{AdminBase}Index.cshtml", Categories);
        }

        // GET: CategoriesView/Details/5
        [HttpGet("{id}/Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _services.GetCategoryAsync(id.Value);
            if (category == null)
                return NotFound();

            return View($"{AdminBase}Details.cshtml", category);
        }

        // GET: CategoriesView/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View($"{AdminBase}Create.cshtml");
        }

        // POST: CategoriesView/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View($"{AdminBase}Create.cshtml", model);
            }

            // Check if a category with the same title already exists
            var existingCategory = await _context.Categories
                                                 .FirstOrDefaultAsync(c => c.Title == model.Title);
            if (existingCategory != null)
            {
                ModelState.AddModelError("Title", "A category with this title already exists.");
                return View($"{AdminBase}Create.cshtml", model);
            }

            var category = new Category
            {
                Title = model.Title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        // GET: ProductsView/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _services.GetCategoryAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            
            return View($"{AdminBase}Edit.cshtml", category);
        }

        // POST: ProductsView/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {

            if (!ModelState.IsValid)
            {                
                return View($"{AdminBase}Edit.cshtml", model);
            }
            await _services.UpdateCategoryAsync(id, model);

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductsView/Delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _services.GetCategoryAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View($"{AdminBase}Delete.cshtml", category);
        }

        // POST: ProductsView/Delete/5
        [HttpPost("delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDelSuccess = await _services.DeleteCategoryAsync(id);
            if (isDelSuccess)
            {
                return RedirectToAction(nameof(Index));

            }
            // TODO
            return NotFound(); // later change to appropirate code
        }
    }
}
