using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Customer
{
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
