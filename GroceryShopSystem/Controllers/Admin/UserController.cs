using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Admin
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
