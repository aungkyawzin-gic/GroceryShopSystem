using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Controllers.User
{
    [Area("Admin")]
    [Route("Admin/Users")]
    public class UsersController : Controller
    {
        private readonly UserApiServices _services;
        private readonly ApplicationDbContext _context; // Optional, if needed for dropdowns or extra validation

        const string AdminBase = "~/Views/Admin/Users/";

        public UsersController(UserApiServices services, ApplicationDbContext context)
        {
            _services = services;
            _context = context;
        }

        // GET: Admin/User
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var users = await _services.GetUsersAsync();
            return View($"{AdminBase}Index.cshtml", users);
        }

        // GET: Admin/User/Details/{id}
        [HttpGet("{id}/Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _services.GetUserAsync(id);
            if (user == null)
                return NotFound();

            return View($"{AdminBase}Details.cshtml", user);
        }

        // GET: Admin/User/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View($"{AdminBase}Create.cshtml");
        }

        // POST: Admin/User/Create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View($"{AdminBase}Create.cshtml", model);
            }

            // Call API service to create user
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            try
            {
                await _services.CreateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating user: {ex.Message}");
                return View($"{AdminBase}Create.cshtml", model);
            }
        }

        // GET: Admin/User/Edit/{id}
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _services.GetUserAsync(id);
            if (user == null)
                return NotFound();

            var model = new UserUpdateViewModel
            {
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber
            };

            return View($"{AdminBase}Edit.cshtml", model);
        }

        // POST: Admin/User/Edit/{id}
        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View($"{AdminBase}Edit.cshtml", model);
            }

            var user = new ApplicationUser
            {
                Id = id,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _services.UpdateUserAsync(id, user);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error updating user.");
                return View($"{AdminBase}Edit.cshtml", model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/User/Delete/{id}
        [HttpGet("{id}/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _services.GetUserAsync(id);
            if (user == null)
                return NotFound();

            return View($"{AdminBase}Delete.cshtml", user);
        }

        // POST: Admin/User/Delete/{id}
        [HttpPost("{id}/delete"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _services.DeleteUserAsync(id);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Error deleting user.");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
