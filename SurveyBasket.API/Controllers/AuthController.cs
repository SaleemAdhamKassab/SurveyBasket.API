using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Auth.Requests;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Services.Auth;

namespace SurveyBasket.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		private readonly IAuthService _authService = authService;

		[HttpPost("Login")]
		public async Task<IActionResult> LoginAsync(LoginRequest request)
		{
			var authResult = await _authService.GetTokenAsync(request.Email, request.Password);
			return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
		}

		[HttpPost("Regiser")]
		public async Task<IActionResult> Regiser(RegisterRequest request)
		{
			var result = await _authService.RegisterAsync(request);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}

		[HttpPost("RefreshToken")]
		public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request)
		{
			var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken);
			return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
		}

		[HttpPost("RevokeRefreshToken")]
		public async Task<IActionResult> RevokeRefreshTokenAsync(RefreshTokenRequest request)
		{
			var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}


		[HttpPost("ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
		{
			var result = await _authService.ConfirmEmailAsync(request);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}


		[HttpPost("ResendConfirmationEmail")]
		public async Task<IActionResult> ResendConfirmationEmail(ResendConfirmationEmailRequest request)
		{
			var result = await _authService.ResendConfirmationEmail(request);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}


		[HttpPost("forgetPassword")]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
		{
			var result = await _authService.SendResetPasswordTokenAsync(request.Email);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}


		[HttpPost("resetPassword")]
		public async Task<IActionResult> resetPassword(ResetPasswordRequest request)
		{
			var result = await _authService.ResetPasswordAsync(request);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}
	}
}
