using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Account
{
	[Route("Account")]
	public class AuthController : Controller
	{
		private readonly AuthApiServices _authApiServices;
		private readonly ILogger<AuthController> _logger;
		const string AccountBase = "~/Views/Account/";
		public AuthController(AuthApiServices authApiServices, ILogger<AuthController> logger)
		{
			_authApiServices = authApiServices;
			_logger = logger;
		}

		//Get: Account/Login - shows the login form
		[HttpGet("Login")]
		public IActionResult Login()
		{
			return View($"{AccountBase}Login.cshtml");
		}

		// POST: Account/Login - handles the form submission
		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View($"{AccountBase}Login.cshtml", loginViewModel);
			}
			try
			{
				var user = await _authApiServices.LoginUserAsync(loginViewModel);
				if (user != null)
				{
					// Successfully logged in
					return RedirectToAction("Index", "Home"); //goes to Home/Index
				}
				else
				{
					// failed case
					ModelState.AddModelError("", "Invalid login attempt. Please check your credentials.");
					return View($"{AccountBase}Login.cshtml", loginViewModel);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user login");
				ModelState.AddModelError("", "An error occurred while processing your request.");
				return View(loginViewModel);
			}
		}

		// POST: /Account/Logout - handles logout
		[HttpPost("Logout")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await _authApiServices.LogoutUserAsync();
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user logout");
				return View("~/Views/Shared/Error.cshtml");
			}
		}
	}
}
