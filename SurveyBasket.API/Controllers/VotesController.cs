using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Votes;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Extensions;
using SurveyBasket.API.Services;

namespace SurveyBasket.API.Controllers
{
	[Route("api/polls/{pollId}/vote")]
	[ApiController]
	[Authorize]
	public class VotesController(IQuestionService questionService, IVoteService voteService) : ControllerBase
	{
		private readonly IQuestionService _questionService = questionService;
		private readonly IVoteService _voteService = voteService;

		[HttpGet]
		public async Task<IActionResult> Start([FromRoute] int pollId)
		{
			//get lggeded userId
			//User in http context because we use [Authorize]
			//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userId = User.GetUserId(); // using our UserExtensions

			var result = await _questionService.GetAvailableAsync(pollId, userId!);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpPost]
		public async Task<IActionResult> Vote([FromRoute] int pollId, VoteRequest request)
		{
			var result = await _voteService.AddAsync(pollId, User.GetUserId()!, request);

			return result.IsSuccess ? Created() : result.ToProblem();
		}
	}
}
