using GroceryShopSystem.Data;
using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Controllers.Customer
{
	public class OrderController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrderController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

	}
}
