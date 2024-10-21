using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Auth;
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
			var result = await _authService.GetTokenAsync(request.Email, request.Password);
			return result.IsSuccess ? Ok(result.Value) : Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.Code, detail: result.Error.Description);
		}

		[HttpPost("RefreshToken")]
		public async Task<IActionResult> refreshToken(RefreshTokenRequest request)
		{
			var authResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken);

			return authResult is null ? BadRequest("Invalid token") : Ok(authResult);
		}

		[HttpPut("RefokeRefreshToken")]
		public async Task<IActionResult> refokeRefreshToken(RefreshTokenRequest request)
		{
			var isRefoked = await _authService.RefokeRefreshTokenAsync(request.token, request.refreshToken);

			return isRefoked ? Ok() : BadRequest();
		}
	}
}
