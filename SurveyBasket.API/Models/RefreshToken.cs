using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.API.Models
{
	[Owned]
	public class RefreshToken
	{
		public string Token { get; set; } = string.Empty;
		public DateTime AddedOn { get; set; }
		public DateTime ExpiresOn { get; set; }
		public DateTime? RevokedOn { get; set; }
		public bool IsExpired => DateTime.UtcNow > ExpiresOn;
		public bool IsActive => RevokedOn is null && !IsExpired;
	}
}
