using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class AnswerConfigurations : IEntityTypeConfiguration<Answer>
	{
		public void Configure(EntityTypeBuilder<Answer> builder)
		{
			builder.HasIndex(e => new { e.QuestionId, e.Content }).IsUnique();
			builder.Property(e => e.Content).HasMaxLength(1000);
		}
	}
}