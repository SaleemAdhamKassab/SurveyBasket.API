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
		[HttpPost]
		public async Task<IActionResult> LoginAsync(LoginRequest request)
		{
			var authResult = await _authService.GetTokenAsync(request.Email, request.Password);

			return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
		}
	}
}
