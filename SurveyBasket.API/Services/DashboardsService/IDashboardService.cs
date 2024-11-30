using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Dashboard.Responses;

namespace SurveyBasket.API.Services.DashboardsService
{
	public interface IDashboardService
	{
		Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId);
		Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId);
		Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId);
	}
}
