namespace SurveyBasket.API.Contracts.Auth
{
	public record RefreshTokenRequest(
		string token,
		string refreshToken
	);
}
