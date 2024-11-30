using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Votes.Requests;

namespace SurveyBasket.API.Services.VotesService
{
	public interface IVoteService
	{
		Task<Result> AddAsync(int pollId, string userId, VoteRequest request);
	}
}
