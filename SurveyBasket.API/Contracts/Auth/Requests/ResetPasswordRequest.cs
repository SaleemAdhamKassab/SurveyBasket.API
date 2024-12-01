namespace SurveyBasket.API.Contracts.Auth.Requests
{
	public class ResetPasswordRequest
	{
		public string Email { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
	}
}
