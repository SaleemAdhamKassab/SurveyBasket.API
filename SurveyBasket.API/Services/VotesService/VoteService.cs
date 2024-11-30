using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Result;
using SurveyBasket.API.Contracts.Votes.Requests;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using SurveyBasket.API.Models.Data;

namespace SurveyBasket.API.Services.VotesService
{

	public class VoteService(ApplicationDbContext db) : IVoteService
	{
		private readonly ApplicationDbContext _db = db;

		public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request)
		{
			var hasVote = await _db.Votes.AnyAsync(e => e.PollId == pollId && e.UserId == userId);

			if (hasVote)
				return Result.Failure(VoteErrors.DuplicatedVote);

			var pollisExsits = await _db.Polls.AnyAsync(e => e.Id == pollId && e.IsPublished);

			if (!pollisExsits)
				return Result.Failure(PollErrors.PollNotFound);

			var avalilableQuestions = await _db.Questions
				.Where(e => e.PollId == pollId && e.IsActive)
				.Select(e => e.Id)
				.ToListAsync();

			if (!request.VoteAnswers.Select(e => e.QuestionId).SequenceEqual(avalilableQuestions))
				return Result.Failure(VoteErrors.InvalidQuestions);

			var vote = new Vote
			{
				PollId = pollId,
				UserId = userId,
				SubmittedOn = DateTime.UtcNow
			};

			request.VoteAnswers.ForEach(
				a => vote.VoteAnswers.Add(new VoteAnswer
				{
					QuestionId = a.QuestionId,
					AnswerId = a.AnswerId
				}));

			await _db.Votes.AddAsync(vote);
			await _db.SaveChangesAsync();

			return Result.Success();
		}
	}
}