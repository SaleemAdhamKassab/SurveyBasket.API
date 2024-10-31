using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using SurveyBasket.API.Services;

namespace SurveyBasket.API.Controllers
{
	[Route("api/polls/{pollId}/[controller]")]
	[ApiController]
	[Authorize]

	public class QuestionsController(IQuestionService questionService) : ControllerBase
	{
		private readonly IQuestionService _questionService = questionService;

		[HttpGet]
		public async Task<IActionResult> GetAll([FromRoute] int pollId)
		{
			var result = await _questionService.GetAllAsync(pollId);

			if (result.IsSuccess)
				return Ok(result.Value);

			return result.ToProblem(StatusCodes.Status404NotFound);

		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] int pollId, int id)
		{
			var result = await _questionService.GetAsync(pollId, id);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status404NotFound);
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request)
		{
			var result = await _questionService.AddAsync(pollId, request);

			if (result.IsSuccess)
				return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

			return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent)
				? result.ToProblem(StatusCodes.Status409Conflict)
				: result.ToProblem(StatusCodes.Status404NotFound);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int pollId, int id, [FromBody] QuestionRequest request)
		{
			var result = await _questionService.UpdateAsync(pollId, id, request);

			if (result.IsSuccess)
				return NoContent();

			return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent)
				? result.ToProblem(StatusCodes.Status409Conflict)
				: result.ToProblem(StatusCodes.Status404NotFound);
		}

		[HttpPut("{id}/ToggleStatus")]
		public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, int id)
		{
			var result = await _questionService.ToggleStatusAsync(pollId, id);
			return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status404NotFound);
		}
	}
}
