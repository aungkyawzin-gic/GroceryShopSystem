using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Account
{
	[Route("Account")]
	public class AuthController(
		AuthApiServices authApiServices,
		ILogger<AuthController> logger,
		SignInManager<ApplicationUser> signInManager,
		UserManager<ApplicationUser> userManager)
		: Controller
	{
		const string AccountBase = "~/Views/Account/";

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
				var apiUser = await authApiServices.LoginUserAsync(loginViewModel);
				Console.WriteLine("API User: " + (apiUser != null ? apiUser.Email : "null"));
                if (apiUser != null)
				{
                    // Successfully logged in
                    if (apiUser.Email != null)
                    {
	                    var user = await userManager.FindByEmailAsync(apiUser.Email);
	                    if (user != null)
	                    {
		                    await signInManager.SignInAsync(user, isPersistent: loginViewModel.RememberMe);
		                    return RedirectToAction("Index", "Home"); //goes Home/Index   
	                    }
                    }
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
				logger.LogError(ex, "Error during user login");
				ModelState.AddModelError("", "An error occurred while processing your request.");
                return View($"{AccountBase}Login.cshtml", loginViewModel);
            }

			return View($"{AccountBase}Login.cshtml", loginViewModel);
		}

		// POST: /Account/Logout - handles logout
		[HttpPost("Logout")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await authApiServices.LogoutUserAsync();
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during user logout");
				return View("~/Views/Shared/Error.cshtml");
			}
		}
	}
}
