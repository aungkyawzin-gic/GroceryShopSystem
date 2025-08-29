
using AuthorizeTesting.Data;
using GroceryShopSystem.Models;
using GroceryShopSystem.Services;
using GroceryShopSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;


namespace GroceryShopSystem.Controllers.Account
{
	[Route("Account")]
	public class AccountController : Controller
	{
		private readonly AccountApiServices _accountApiServices;
		private readonly ILogger<AccountController> _logger;
		SignInManager<ApplicationUser> _signInManager;
		const string AccountBase = "~/Views/Account/";
		public AccountController(AccountApiServices accountApiServices, ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager)
		{
			_accountApiServices = accountApiServices;
			_logger = logger;
			_signInManager = signInManager;
		}

		//Get: Account/Register - shows the registration form
		[HttpGet("Register")]
		public IActionResult Register()
		{
			return View($"{AccountBase}Register.cshtml");
		}

		// POST: Account/Register - handles the form submission
		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View($"{AccountBase}Register.cshtml", registerViewModel);
			}
			try
			{
				var newUser = await _accountApiServices.RegisterUserAsync(registerViewModel);
				if (newUser != null)
				{
					// Successfully registered and logged in
					await _signInManager.SignInAsync(newUser, isPersistent: false);
					return RedirectToAction("Index", "Home"); //goes to Home/Index
				}
				else
				{
					// failed case
					ModelState.AddModelError("", "Registration failed. Try again.");
					return View($"{AccountBase}Register.cshtml", registerViewModel);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user registration");
				ModelState.AddModelError("", "An error occurred while processing your request.");
				return View(registerViewModel);
			}
		}
	}
}
