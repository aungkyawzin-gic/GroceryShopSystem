using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryShopSystem.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Full name is required.")]
		[StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
		public string? FullName { get; set; }

		[DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }

		public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

		[ForeignKey("Address")]
		public int? AddressId { get; set; }
	}
}
