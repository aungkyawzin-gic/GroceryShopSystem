
using AuthorizeTesting.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;


namespace GroceryShopSystem.Controllers.Account
{
	[Route("Account/[action]")]
	public class AccountController : Controller
	{

		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login() => View();

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid) return View(loginViewModel);

			var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

			if (user != null)
			{
				//User is found, check password
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
				if (passwordCheck)
				{
					//Password correct, sign in
					var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
					if (result.Succeeded)
					{
						//Check Role and Redirect
						if(await _userManager.IsInRoleAsync(user, UserRoles.Admin))
							return Ok(user);
						//return RedirectToAction("Index", "Admin"); //goes to Admin/Index
						else if(await _userManager.IsInRoleAsync(user, UserRoles.User))
							return Ok(user);
						//return RedirectToAction("Index", "Home"); //goes to Home/Index

						//Fallback in case of no roles
						return RedirectToAction("Index", "Home");
					}
				}
				//Password is incorrect
				TempData["Error"] = "Wrong credentials. Please try again";
				return View(loginViewModel);
			}

			//User not found
			TempData["Error"] = "Wrong credentials. Please try again";
			return View(loginViewModel);
		}

		[HttpGet]
		public IActionResult Register()=> View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid) return View(registerViewModel);

			var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
			if (user != null)
			{
				TempData["Error"] = "This email address is already in use";
				return View(registerViewModel);
			}

			var newUser = new ApplicationUser()
			{
				Email = registerViewModel.Email,
				UserName = registerViewModel.Email,
			};
			var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

			if (newUserResponse.Succeeded)
			{
				await _userManager.AddToRoleAsync(newUser, UserRoles.User);

				//  Automatically log them in
				await _signInManager.SignInAsync(newUser, isPersistent: false);
				return Ok(newUser);

				//return RedirectToAction("Index", "Home"); //goes to Home/Index
			}

			// Show errors if creation failed
			foreach (var error in newUserResponse.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(registerViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
