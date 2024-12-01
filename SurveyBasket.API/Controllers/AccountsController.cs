using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Extensions;
using SurveyBasket.API.Services.UsersService;

namespace SurveyBasket.API.Controllers
{
	[Route("me")]
	[ApiController]
	[Authorize]
	public class AccountsController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;

		[HttpGet]
		public async Task<IActionResult> UserProfile()
		{
			var result = await _userService.GetProfileAsync(User.GetUserId()!);

			return Ok(result.Value);
		}


		[HttpPut("info")]
		public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
		{
			var result = await _userService.UpdateProfileAsync(User.GetUserId()!, request);

			return Ok();
		}


		[HttpPut("changePassword")]
		public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
		{
			var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);

			return result.IsSuccess ? Ok() : result.ToProblem();
		}
	}
}
