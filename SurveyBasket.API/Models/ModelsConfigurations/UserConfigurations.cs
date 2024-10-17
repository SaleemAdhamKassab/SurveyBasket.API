using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(e => e.FirstName).HasMaxLength(100);
			builder.Property(e => e.LastName).HasMaxLength(100);
		}
	}
}
