using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Auth.Filters;
using SurveyBasket.API.Contracts.Common;
using SurveyBasket.API.Contracts.Questions.Requests;
using SurveyBasket.API.Services.QuestionsService;

namespace SurveyBasket.API.Controllers
{
	[Route("api/polls/{pollId}/[controller]")]
	[ApiController]

	public class QuestionsController(IQuestionService questionService) : ControllerBase
	{
		private readonly IQuestionService _questionService = questionService;

		[HttpGet]
		[HasPermission(Permissions.GetQuestions)]
		public async Task<IActionResult> GetAll([FromRoute] int pollId, [FromQuery] RequestFilters filters)
		{
			var result = await _questionService.GetAllAsync(pollId, filters);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpGet("{id}")]
		[HasPermission(Permissions.GetQuestions)]
		public async Task<IActionResult> Get([FromRoute] int pollId, int id)
		{
			var result = await _questionService.GetAsync(pollId, id);

			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}

		[HttpPost]
		[HasPermission(Permissions.AddQuestions)]
		public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request)
		{
			var result = await _questionService.AddAsync(pollId, request);

			if (result.IsSuccess)
				return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

			return result.ToProblem();
		}

		[HttpPut("{id}")]
		[HasPermission(Permissions.UpdateQuestions)]
		public async Task<IActionResult> Update([FromRoute] int pollId, int id, [FromBody] QuestionRequest request)
		{
			var result = await _questionService.UpdateAsync(pollId, id, request);

			return result.IsSuccess ? NoContent() : result.ToProblem();
		}

		[HttpPut("{id}/ToggleStatus")]
		[HasPermission(Permissions.UpdateQuestions)]
		public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, int id)
		{
			var result = await _questionService.ToggleStatusAsync(pollId, id);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
	}
}
