using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.Data;

namespace GroceryShopSystem.Controllers.Customer
{
    [Area("Customer")]
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly ProductsApiServices _services;

        public ProductsController(ProductsApiServices services)
        {
            _services = services;
        }

        [HttpGet("{id}/Details")]
        public async Task<IActionResult> Index(int id)
        {
            var product = await _services.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View($"~/Views/Products/Index.cshtml", product);

        }
    }
}
