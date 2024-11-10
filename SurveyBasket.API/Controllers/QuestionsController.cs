using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Errors;
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

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] int pollId, int id)
		{
			var result = await _questionService.GetAsync(pollId, id);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request)
		{
			var result = await _questionService.AddAsync(pollId, request);

			if (result.IsSuccess)
				return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

			return result.ToProblem();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int pollId, int id, [FromBody] QuestionRequest request)
		{
			var result = await _questionService.UpdateAsync(pollId, id, request);

			return result.IsSuccess ? NoContent() : result.ToProblem();
		}

		[HttpPut("{id}/ToggleStatus")]
		public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, int id)
		{
			var result = await _questionService.ToggleStatusAsync(pollId, id);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
	}
}
