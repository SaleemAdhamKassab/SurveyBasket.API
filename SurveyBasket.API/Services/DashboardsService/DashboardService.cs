using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Result;
using SurveyBasket.API.Contracts.Dashboard.Responses;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models.Data;

namespace SurveyBasket.API.Services.DashboardsService
{
	public class DashboardService(ApplicationDbContext db) : IDashboardService
	{
		private readonly ApplicationDbContext _db = db;

		public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId)
		{
			var pollVotes = await _db.Polls
				.Where(e => e.Id == pollId && e.IsPublished)
				.Select(e => new PollVotesResponse
				{
					PollTitle = e.Title,
					VoteResponses = e.Votes
									.Select(v => new VoteResponse
									{
										UserFullName = v.User.FirstName + " " + v.User.LastName,
										Date = v.SubmittedOn,
										SelectedAnswers = v.VoteAnswers
														.Select(va => new QuestionAnswerResponse
														{
															Question = va.Question.Content,
															Answer = va.Answer.Content
														})
									})
				})
				.FirstOrDefaultAsync();

			return pollVotes is null
				? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
				: Result.Success(pollVotes);
		}

		public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId)
		{
			var pollIsExists = await _db.Polls.AnyAsync(e => e.Id == pollId);

			if (!pollIsExists)
				return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

			var votesPerDay = await _db.Votes
				.Where(e => e.PollId == pollId)
				.GroupBy(g => DateOnly.FromDateTime(g.SubmittedOn))
				.Select(e => new VotesPerDayResponse
				{
					Date = e.Key,
					NumberOfVotes = e.Count()
				})
				.ToListAsync();

			return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
		}

		public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId)
		{
			var pollIsExists = await _db.Polls.AnyAsync(e => e.Id == pollId);

			if (!pollIsExists)
				return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);


			var votesPerQuestion = await _db.VoteAnswers
				.Where(e => e.Vote.PollId == pollId)
				.GroupBy(g => g.Question.Content)
				.Select(e => new VotesPerQuestionResponse
				{
					Question = e.Key,
					SelectedAnswers = e.GroupBy(a => a.Answer.Content)
										.Select(ans => new VotesPerAnswerResponse
										{
											Answer = ans.Key,
											Count = ans.Count()
										})
				})
				.ToListAsync();

			return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);
		}
	}
}