using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(e => e.FirstName).HasMaxLength(100);
			builder.Property(e => e.LastName).HasMaxLength(100);

			builder.OwnsMany(e => e.RefreshTokens)
				.ToTable("RefreshTokens")
				.WithOwner()
				.HasForeignKey("UserId");


			//Default Admin

			var passwordHasher = new PasswordHasher<ApplicationUser>();

			builder.HasData(new ApplicationUser
			{
				Id = DefaultUsers.AdminId,
				FirstName = "Survey Basket",
				LastName = "Admin",
				UserName = DefaultUsers.AdminEmail,
				NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
				Email = DefaultUsers.AdminEmail,
				NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
				SecurityStamp = DefaultUsers.AdminSecurityStamp,
				ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
				EmailConfirmed = true,
				PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
			});
		}
	}
}
