namespace SurveyBasket.API.Contracts.Auth.Requests
{
	public class RefreshTokenRequest
	{
		public string Token { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = string.Empty;
	}
}
