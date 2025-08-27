using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.API
{
	public class OrdersApiController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
