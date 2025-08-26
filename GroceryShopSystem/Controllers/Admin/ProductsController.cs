using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;

namespace GroceryShopSystem.Controllers.Admin
{
    [Route("Admin/Products")]
    public class ProductsController : Controller
    {
        private readonly ProductsApiServices _services;

        const string AdminBase = "~/Views/Admin/Products/";

        public ProductsController(ProductsApiServices services)
        {
            _services = services;
        }

        [HttpGet("")]
        // GET: ProductsView
        public async Task<IActionResult> Index()
        {
            var products = await _services.GetProductsAsync();
            return View($"{AdminBase}Index.cshtml", products);
        }

        // GET: ProductsView/Details/5
        [HttpGet("{id}/Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _services.GetProductAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View($"{AdminBase}Details.cshtml", product);
        }

        //// GET: ProductsView/Create
        //[HttpGet("create")]
        //public IActionResult Create()
        //{
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
        //    return View();
        //}

        //// POST: ProductsView/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost("create")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Description,Price,ImageUrl,CreatedAt,UpdatedAt,IsActive,Quantity")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
        //    return View(product);
        //}

        //// GET: ProductsView/Edit/5
        //[HttpGet("edit/{id}")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
        //    return View(product);
        //}

        //// POST: ProductsView/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost("edit/{id}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Description,Price,ImageUrl,CreatedAt,UpdatedAt,IsActive,Quantity")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
        //    return View(product);
        //}

        //// GET: ProductsView/Delete/5
        //[HttpGet("delete/{id}")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: ProductsView/Delete/5
        //[HttpPost("delete/{id}"), ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Products.Remove(product);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
