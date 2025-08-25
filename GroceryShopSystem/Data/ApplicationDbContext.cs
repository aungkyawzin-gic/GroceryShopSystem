using GroceryShopSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
		public DbSet<Address> Addresses { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Rename tables
			builder.Entity<ApplicationUser>(b => b.ToTable("Users"));
			builder.Entity<IdentityRole>(b => b.ToTable("Roles"));
			builder.Entity<IdentityUserRole<string>>(b => b.ToTable("UserRoles"));
			builder.Entity<IdentityUserClaim<string>>(b => b.ToTable("UserClaims"));
			builder.Entity<IdentityUserLogin<string>>(b => b.ToTable("UserLogins"));
			builder.Entity<IdentityRoleClaim<string>>(b => b.ToTable("RoleClaims"));
			builder.Entity<IdentityUserToken<string>>(b => b.ToTable("UserTokens"));

			//Map boolean columns to NUMBER(1)
			builder.Entity<ApplicationUser>(entity =>
			{
				entity.Property(e => e.EmailConfirmed).HasColumnType("NUMBER(1)");
				entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("NUMBER(1)");
				entity.Property(e => e.TwoFactorEnabled).HasColumnType("NUMBER(1)");
				entity.Property(e => e.LockoutEnabled).HasColumnType("NUMBER(1)");
			});
		}
	}
}
