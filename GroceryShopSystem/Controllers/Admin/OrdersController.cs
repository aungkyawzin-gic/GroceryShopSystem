using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Admin
{
	public class OrdersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
