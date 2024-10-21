using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Contracts.Auth
{
	public class JwtOptions
	{
		public static string SectionName = "Jwt";
		[Required]
		public string Key { get; set; } = string.Empty;
		[Required]
		public string Issuer { get; set; } = string.Empty;
		[Required]
		public string Audience { get; set; } = string.Empty;
		[Range(1, 30)]
		public int ExpiryMinutes { get; set; }
	}
}
