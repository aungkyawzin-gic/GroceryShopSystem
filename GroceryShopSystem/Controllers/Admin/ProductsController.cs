using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;

using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GroceryShopSystem.Controllers.Admin
{

    [Area("Admin")]
    [Route("Admin/Products")]
    public class ProductsController : Controller

    {
        private readonly ProductsApiServices _productServices;
        private readonly CategoriesApiServices _categoryServices;

        const string AdminBase = "~/Views/Admin/Products/";

        public ProductsController(ProductsApiServices services, CategoriesApiServices categoryServices)
        {
            _productServices = services;
            _categoryServices = categoryServices;
        }


        [HttpGet("")]
        // GET: ProductsView
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetProductsAsync();
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

            var product = await _productServices.GetProductAsync(id.Value);
            if (product == null)
                return NotFound();

            return View($"{AdminBase}Details.cshtml", product);
        }

        // GET: ProductsView/Create
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryServices.GetCategoriesAsync(), "Id", "Title");
            return View($"{AdminBase}Create.cshtml");
        }

        // POST: ProductsView/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryServices.GetCategoriesAsync(), "Id", "Title", model.CategoryId);
                return View($"{AdminBase}Create.cshtml", model);
            }

            string? imageUrl = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imageUrl = "/uploads/" + fileName;
            }

            var product = new Product
            {
                CategoryId = model.CategoryId,
                Category = await _categoryServices.GetCategoryAsync(model.CategoryId),
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productServices.CreateProductAsync(product);
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

            var product = await _productServices.GetProductAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _categoryServices.GetCategoriesAsync(), "Id", "Title", product.CategoryId);

            var productViewModel = new ProductEditViewModel
            {
                Id = product.Id,
                
                CategoryId = product.CategoryId,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity                
            };
            ViewBag.ImageUrl = product.ImageUrl;
            return View($"{AdminBase}Edit.cshtml", productViewModel);            
        }

        // POST: ProductsView/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryServices.GetCategoriesAsync(), "Id", "Title", model.CategoryId);
                return View($"{AdminBase}Edit.cshtml", model);
            }

            string? imageUrl = null;

            if (model.ImageFile != null &&  model.ImageFile.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = Path.GetFileName( model.ImageFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await  model.ImageFile.CopyToAsync(stream);
                }

                imageUrl = "/uploads/" + fileName;
            }

            var product = new Product
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Category = await _categoryServices.GetCategoryAsync(model.CategoryId),
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productServices.UpdateProductAsync(id, product);
            return RedirectToAction(nameof(Index));            
        }

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
