using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
	{
		public void Configure(EntityTypeBuilder<VoteAnswer> builder)
		{
			builder.HasIndex(e => new { e.VoteId, e.QuestionId }).IsUnique();
		}
	}
}