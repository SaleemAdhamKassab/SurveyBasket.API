using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Result;
using SurveyBasket.API.Contracts.Polls.Requests;
using SurveyBasket.API.Contracts.Polls.Responses;

namespace SurveyBasket.API.Services.PollsService
{
	public interface IPollService
	{
		Task<Result<PollResponse>> GetAsync(int id);
		Task<List<PollResponse>> GetAllAsync();
		Task<List<PollResponse>> GetCurrentAsync();
		Task<Result<PollResponse>> AddAsync(PollRequest request);
		Task<Result> UpdateAsync(int id, PollRequest request);
		Task<Result> DeleteAsync(int id);
		Task<Result> TogglePublishStatusAsync(int id);
	}
}
