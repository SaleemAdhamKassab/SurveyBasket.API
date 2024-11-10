using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class VoteConfiguration : IEntityTypeConfiguration<Vote>
	{
		public void Configure(EntityTypeBuilder<Vote> builder)
		{
			builder.HasIndex(e => new { e.PollId, e.UserId }).IsUnique();
		}
	}
}