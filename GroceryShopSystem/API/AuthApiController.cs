using AuthorizeTesting.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.API
{
	[Route("api/auth")]
	public class AuthApiController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AuthApiController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		// POST: api/account/login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { message = "Invalid login data." });
			}

			var result = await _signInManager.PasswordSignInAsync(
				model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				var user = await _userManager.Users
					.FirstOrDefaultAsync(u => u.Email == model.Email);

				return Ok(new
				{
					message = "Login successful.",
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FullName = user.FullName
                });
			}

			return Unauthorized(new { message = "Invalid email or password." });
		}

		// POST: api/auth/logout
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok(new { message = "Logged out successfully." });
		}
	}
}
