using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Polls;
using SurveyBasket.API.Models;
using SurveyBasket.API.Services;

namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PollsController(IPollService pollService) : ControllerBase
	{
		private readonly IPollService _pollService = pollService;

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			List<Poll> polls = await _pollService.GetAllAsync();
			List<PollResponse> result = polls.Adapt<List<PollResponse>>();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			Poll poll = await _pollService.GetAsync(id);

			if (poll is null)
				return NotFound();

			var response = poll.Adapt<PollResponse>();

			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Add(PollRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
				return BadRequest();

			Poll createdPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);
			return CreatedAtAction(nameof(Get), new { id = createdPoll.Id }, createdPoll.Adapt<PollResponse>());
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, PollRequest request)
		{
			bool isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>());
			return !isUpdated ? NotFound() : NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			bool isDeleted = await _pollService.DeleteAsync(id);
			return !isDeleted ? NotFound() : NoContent();
		}

		[HttpPut("{id}/TogglePublish")]
		public async Task<IActionResult> TogglePublish(int id)
		{
			bool isToggled = await _pollService.TogglePublishStatusAsync(id);
			return !isToggled ? NotFound() : NoContent();
		}
	}
}
