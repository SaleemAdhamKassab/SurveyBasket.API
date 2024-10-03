using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Model;
using SurveyBasket.API.Services;

namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PollsController(IPollService pollService) : ControllerBase
	{
		private readonly IPollService _pollService = pollService;

		[HttpGet]
		public IActionResult GetAll() => Ok(_pollService.GetAll());


		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			Poll poll = _pollService.Get(id);

			if (poll is null)
				return NotFound($"invalid poll Id: {id}");
			return Ok(poll);
		}

		[HttpPost]
		public IActionResult Add(Poll poll)
		{
			if (poll is null)
				return BadRequest();

			Poll createdPoll = _pollService.Add(poll);

			return CreatedAtAction(nameof(Get), new { id = createdPoll.Id }, createdPoll);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Poll poll)
		{
			bool isUpdated = _pollService.Update(id,poll);

			return !isUpdated ? NotFound() : NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			bool isDeleted = _pollService.Delete(id);

			return !isDeleted ? NotFound() : NoContent();
		}
	}
}
