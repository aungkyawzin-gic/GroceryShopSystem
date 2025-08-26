using AuthorizeTesting.Data;
using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace GroceryShopSystem.Data
{
	public static class DbInitializer
	{
		public static async Task SeedRolesAndAdmin(UserManager<ApplicationUser> userManager,
												   RoleManager<IdentityRole> roleManager)
		{
			// Seed Admin role
			if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
			}

			// Seed User role
			if (!await roleManager.RoleExistsAsync(UserRoles.User))
			{
				await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
			}

			// Seed default admin user
			var adminEmail = "admin@gmail.com";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);
			if (adminUser == null)
			{
				adminUser = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					FullName = "Admin",
					DateOfBirth = null,
					PhoneNumber = ""
				};

				await userManager.CreateAsync(adminUser, "Admin123!");
				await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
			}
		}
	}
}
