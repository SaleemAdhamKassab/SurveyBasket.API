using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Auth.Filters;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Services.UsersService;

namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;


		[HttpGet]
		[HasPermission(Permissions.GetUsers)]
		public async Task<IActionResult> GetAll()
		{
			var users = await _userService.GetAllAsync();
			return Ok(users);
		}


		[HttpGet("{id}")]
		[HasPermission(Permissions.GetUsers)]
		public async Task<IActionResult> Get([FromRoute] string id)
		{
			var result = await _userService.GetAsync(id);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}


		[HttpPost]
		[HasPermission(Permissions.AddUsers)]
		public async Task<IActionResult> Add(CreateUserRequest request)
		{
			var result = await _userService.AddAsync(request);

			return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value) : result.ToProblem();
		}


		[HttpPut("{userId}")]
		[HasPermission(Permissions.UpdateUsers)]
		public async Task<IActionResult> Update([FromRoute] string userId, UpdateUserRequest request)
		{
			var result = await _userService.UpdateAsync(userId, request);

			return result.IsSuccess ? NoContent() : result.ToProblem();
		}


		[HttpPut("{id}/toggleStatus")]
		[HasPermission(Permissions.UpdateUsers)]
		public async Task<IActionResult> ToggleStatus([FromRoute] string id)
		{
			var result = await _userService.ToggleStatusAsync(id);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}

		[HttpPut("{id}/unlock")]
		[HasPermission(Permissions.UpdateUsers)]
		public async Task<IActionResult> Unlock([FromRoute] string id)
		{
			var result = await _userService.UnlockAsync(id);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
	}
}