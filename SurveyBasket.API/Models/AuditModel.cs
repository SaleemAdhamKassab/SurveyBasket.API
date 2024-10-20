namespace SurveyBasket.API.Models
{
	public class AuditModel
	{
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		public DateTime? UpdateOn { get; set; }

		public string CreatedById { get; set; } = string.Empty;
		public ApplicationUser CreatedBy { get; set; } = default!;

		public string? UpdatedById { get; set; }
		public ApplicationUser? UpdatedBy { get; set; }
	}
}
