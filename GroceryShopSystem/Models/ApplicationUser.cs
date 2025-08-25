using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string? FullName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
		[ForeignKey("Address")]
		public int? AddressId { get; set; }
	}
}
