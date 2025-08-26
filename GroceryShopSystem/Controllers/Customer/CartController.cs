using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GroceryShopSystem.Controllers.Customer
{
    public class CartController : Controller
    {
        private readonly ProductsApiServices _services;

        public CartController(ProductsApiServices services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            // Mock cart
            var cart = new Cart
            {
                Id = 1,
                UserId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Mock products
            var product1 = new Product
            {
                Id = 101,
                CategoryId = 1,
                Title = "Apple",
                Price = 1.5m,
                Description = "Fresh red apples",
                ImageUrl = "/images/apple.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Quantity = 100
            };

            var product2 = new Product
            {
                Id = 102,
                CategoryId = 1,
                Title = "Banana",
                Price = 0.8m,
                Description = "Ripe bananas",
                ImageUrl = "/images/banana.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Quantity = 50
            };

            // Mock cart items
            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = 1,
                    CartId = cart.Id,
                    Cart = cart,
                    ProductId = product1.Id,
                    Product = product1,
                    Quantity = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CartItem
                {
                    Id = 2,
                    CartId = cart.Id,
                    Cart = cart,
                    ProductId = product2.Id,
                    Product = product2,
                    Quantity = 5,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            return View(cartItems);
        }
    }
}
