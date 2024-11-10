using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.API.Models
{
	public sealed class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
		public ICollection<Vote> Votes { get; set; } = [];
	}
}
