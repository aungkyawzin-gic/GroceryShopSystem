using AuthorizeTesting.Data;
using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.API
{
	[Route("api/account")]
	[ApiController]
	public class AccountApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountApiController(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		// POST: api/account/register
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { message = "Invalid registration data." });
			}

			var existingUser = await _userManager.FindByEmailAsync(model.Email);
			if (existingUser != null)
			{
				return Conflict(new { message = "This email address is already in use." });
			}

			// Generate full name if null
			string fullName = model.FullName;
			if (string.IsNullOrWhiteSpace(fullName))
			{
				// take first part before "@"
				var emailPrefix = model.Email.Split('@')[0];

				// remove trailing numbers
				fullName = new string(emailPrefix.TakeWhile(c => !char.IsDigit(c)).ToArray());

				// capitalize first letter
				if (!string.IsNullOrEmpty(fullName))
				{
					fullName = char.ToUpper(fullName[0]) + fullName.Substring(1).ToLower();
				}
				else
				{
					fullName = model.Email; // fallback
				}
			}

			var newUser = new ApplicationUser
			{
				Email = model.Email,
				UserName = model.Email,
				FullName = fullName,
			};

			var result = await _userManager.CreateAsync(newUser, model.Password);

			if (result.Succeeded)
			{
				// assign default role
				await _userManager.AddToRoleAsync(newUser, UserRoles.User);

				// sign in automatically
				await _signInManager.SignInAsync(newUser, isPersistent: false);

				return Ok(new
				{
					message = "User registered successfully.",
					 Id = newUser.Id,
                    Email = newUser.Email,
                    UserName = newUser.UserName,
                    FullName = newUser.FullName
				});
			}

			// if failed, collect errors
			var errors = result.Errors.Select(e => e.Description).ToList();
			return BadRequest(new { message = "Registration failed.", errors });
		}		
	}
}
