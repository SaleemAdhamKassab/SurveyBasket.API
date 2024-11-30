namespace SurveyBasket.API.Contracts.Auth.Responses
{
	public class AuthResponse
	{
		public string Id { get; set; } = string.Empty;
		public string? Email { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
		public int ExpiresIn { get; set; }
		public string RefreshToken { get; set; } = string.Empty;
		public DateTime RefreshTokenExpiration { get; set; }
	}
}
