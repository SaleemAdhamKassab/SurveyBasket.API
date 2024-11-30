namespace SurveyBasket.API.Contracts.Auth.Requests
{
	public class ConfirmEmailRequest
	{
		public string UserId { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
