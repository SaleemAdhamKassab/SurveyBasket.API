using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Common;
using SurveyBasket.API.Contracts.Questions.Requests;
using SurveyBasket.API.Contracts.Questions.Responses;

namespace SurveyBasket.API.Services.QuestionsService
{
	public interface IQuestionService
	{
		Task<Result<QuestionResponse>> GetAsync(int pollId, int id);
		Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters);
		Task<Result<List<QuestionResponse>>> GetAvailableAsync(int pollId, string userId);
		Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
		Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request);
		Task<Result> ToggleStatusAsync(int pollId, int id);
	}

}
