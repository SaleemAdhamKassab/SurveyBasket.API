using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Contracts.Auth
{
	public class JwtOptions
	{
		public static string SectionName = "Jwt";
		[Required]
		public string Key { get; set; }
		[Required]
		public string Issuer { get; set; }
		[Required]
		public string Audience { get; set; }
		[Range(1, 30)]
		public int ExpiryMinutes { get; set; }
	}
}
