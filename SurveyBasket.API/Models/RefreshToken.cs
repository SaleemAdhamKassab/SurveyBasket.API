using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.API.Models
{
	[Owned]
	public class RefreshToken
	{
		public string Token { get; set; }
		public DateTime AddedOn { get; set; }
		public DateTime ExpiresOn { get; set; }
		public DateTime? RefokedOn { get; set; }
		public bool IsExpired => DateTime.UtcNow > ExpiresOn;
		public bool IsActive => RefokedOn is null && !IsExpired;
	}
}
