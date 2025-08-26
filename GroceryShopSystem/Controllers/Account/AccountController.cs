using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Account
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
