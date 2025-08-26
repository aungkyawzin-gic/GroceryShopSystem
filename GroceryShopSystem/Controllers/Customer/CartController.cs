using Microsoft.AspNetCore.Mvc;

namespace GroceryShopSystem.Controllers.Customer
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
