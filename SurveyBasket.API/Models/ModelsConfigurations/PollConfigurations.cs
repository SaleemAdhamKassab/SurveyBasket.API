using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class PollConfigurations : IEntityTypeConfiguration<Poll>
	{
		public void Configure(EntityTypeBuilder<Poll> builder)
		{
			builder.HasIndex(e => e.Title).IsUnique();
			builder.Property(e => e.Title).HasMaxLength(100);
			builder.Property(e => e.Summary).HasMaxLength(1500);
		}
	}
}
