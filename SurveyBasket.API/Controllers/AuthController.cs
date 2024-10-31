using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Auth;
using SurveyBasket.API.Services.Auth;

namespace SurveyBasket.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
	{
		private readonly IAuthService _authService = authService;
		private readonly ILogger<AuthController> _logger = logger;

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
		{
			var authResult = await _authService.GetTokenAsync(request.Email, request.Password);
			return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem(StatusCodes.Status400BadRequest);
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request)
		{
			var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken);
			return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem(StatusCodes.Status400BadRequest);
		}

		[HttpPost("revoke-refresh-token")]
		public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request)
		{
			var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken);

			return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
		}
	}
}
