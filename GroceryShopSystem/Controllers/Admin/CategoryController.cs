using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryShopSystem.Data;
using GroceryShopSystem.Models;

namespace GroceryShopSystem.Controllers.Admin
{
    [Route("Categories")]
    public class CategoryController : Controller
    {

        const string AdminBase = "~/Views/Admin/Category/";
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: admin/categories
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            //return View("~/Views/Admin/Category/Index.cshtml", await _context.Category.ToListAsync());
            return View($"{AdminBase}Index.cshtml", await _context.Category.ToListAsync());
        }

        // GET: admin/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();
            return View($"{AdminBase}Details.cshtml", category);
        }

        // GET: admin/categories/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View($"{AdminBase}Create.cshtml");
        }

        // POST: admin/categories/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedAt,UpdatedAt")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (_context.Category.Count(c => c.Title == category.Title) > 0)
                {
                    ModelState.AddModelError("Title", "This category name already exists.");
                    return View($"{AdminBase}Create.cshtml", category);
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: admin/categories/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();
            return View($"{AdminBase}Edit.cshtml", category);
        }

        // POST: admin/categories/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreatedAt,UpdatedAt")] Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Category.Count(c => c.Title == category.Title) > 0)
                    {
                        ModelState.AddModelError("Title", "This category name already exists.");
                        return View($"{AdminBase}Create.cshtml", category);
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: admin/categories/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();
            return View($"{AdminBase}Delete.cshtml", category);
        }

        // POST: admin/categories/delete/5
        [HttpPost("delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null) _context.Category.Remove(category);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
