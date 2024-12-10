using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Polls.Requests;
using SurveyBasket.API.Services.PollsService;

namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class PollsController(IPollService pollService) : ControllerBase
	{
		private readonly IPollService _pollService = pollService;


		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _pollService.GetAsync(id);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _pollService.GetAllAsync();
			return Ok(result);
		}


		[HttpGet("current")]
		[Authorize(Roles = DefaultRoles.Member)]
		public async Task<IActionResult> Current()
		{
			var result = await _pollService.GetCurrentAsync();
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Add(PollRequest request)
		{
			var result = await _pollService.AddAsync(request);
			return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, PollRequest request)
		{
			var result = await _pollService.UpdateAsync(id, request);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _pollService.DeleteAsync(id);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}

		[HttpPut("{id}/TogglePublish")]
		public async Task<IActionResult> TogglePublish(int id)
		{
			var result = await _pollService.TogglePublishStatusAsync(id);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
	}
}
