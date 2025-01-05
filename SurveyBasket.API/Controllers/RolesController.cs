using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Auth.Filters;
using SurveyBasket.API.Contracts.Roles.Requests;
using SurveyBasket.API.Services.RolesService;

namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController(IRoleService roleService) : ControllerBase
	{
		private readonly IRoleService _roleService = roleService;

		[HttpGet]
		[HasPermission(Permissions.GetRoles)]
		public async Task<IActionResult> GetAll([FromQuery] bool includeDisables)
		{
			var result = await _roleService.GetAllAsync(includeDisables);

			return Ok(result);
		}


		[HttpGet("{id}")]
		[HasPermission(Permissions.GetRoles)]
		public async Task<IActionResult> Get([FromRoute] string id)
		{
			var result = await _roleService.GetAsync(id);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}


		[HttpPost]
		[HasPermission(Permissions.AddRoles)]
		public async Task<IActionResult> Add(CreateRoleRequest request)
		{
			var result = await _roleService.AddAsync(request);

			return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value) : result.ToProblem();
		}

		[HttpPut("{id}")]
		[HasPermission(Permissions.UpdateRoles)]
		public async Task<IActionResult> Update([FromRoute] string id, CreateRoleRequest request)
		{
			var result = await _roleService.UpdateAsync(id, request);

			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
	}
}
