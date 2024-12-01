using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Auth.Requests;
using SurveyBasket.API.Contracts.Auth.Responses;

namespace SurveyBasket.API.Services.Auth
{
	public interface IAuthService
	{
		Task<Result<AuthResponse>> GetTokenAsync(string email, string password);
		Task<Result> RegisterAsync(RegisterRequest request);
		Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken);
		Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken);
		Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
		Task<Result> ResendConfirmationEmail(ResendConfirmationEmailRequest request);
		Task<Result> SendResetPasswordTokenAsync(string email);
		Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
	}
}
