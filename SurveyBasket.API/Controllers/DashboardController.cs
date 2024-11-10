using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Services;

namespace SurveyBasket.API.Controllers
{
	[Route("api/polls/{pollId}/[controller]")]
	[ApiController]
	[Authorize]
	public class DashboardController(IDashboardService dashboardService) : ControllerBase
	{

		private readonly IDashboardService _dashboardService = dashboardService;

		[HttpGet("row-data")]
		public async Task<IActionResult> PollVotes([FromRoute] int pollId)
		{
			var result = await _dashboardService.GetPollVotesAsync(pollId);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpGet("votes-per-day")]
		public async Task<IActionResult> VotesPerDay([FromRoute] int pollId)
		{
			var result = await _dashboardService.GetVotesPerDayAsync(pollId);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpGet("votes-per-question")]
		public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollId)
		{
			var result = await _dashboardService.GetVotesPerQuestionAsync(pollId);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
