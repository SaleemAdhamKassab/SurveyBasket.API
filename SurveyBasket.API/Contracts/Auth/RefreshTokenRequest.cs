namespace SurveyBasket.API.Contracts.Auth
{
	public record RefreshTokenRequest(
		string Token,
		string RefreshToken
	);
}
