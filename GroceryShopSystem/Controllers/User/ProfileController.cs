using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("User/profile")]
public class ProfileController : Controller
{
    private readonly UserApiServices _userService;

    public ProfileController(UserApiServices userService)
    {
        _userService = userService;
    }

    // GET: /User/profile
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound();

        var model = new UserUpdateViewModel
        {
            FullName = user.FullName,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }

    // POST: /User/profile
    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(UserUpdateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var updatedUser = new ApplicationUser
        {
            Id = userId,
            FullName = model.FullName,
            DateOfBirth = model.DateOfBirth,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userService.UpdateUserAsync(userId, updatedUser);

        if (result)
        {
            TempData["Message"] = "Profile updated successfully!";
            // Redirect to the same GET action
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "Failed to update profile. Please try again.");
        return View(model);
    }
}
