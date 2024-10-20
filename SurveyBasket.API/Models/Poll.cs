using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Models
{
	public class Poll : AuditModel
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Summary { get; set; } = string.Empty;
		public bool IsPublished { get; set; }
		public DateOnly StartsAt { get; set; }
		public DateOnly EndsAt { get; set; }
	}
}
