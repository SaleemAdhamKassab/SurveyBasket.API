using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Polls;
using SurveyBasket.API.Data;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services
{
	public interface IPollService
	{
		Task<Result<PollResponse>> GetAsync(int id);
		Task<Result<List<PollResponse>>> GetAllAsync();
		Task<Result<PollResponse>> AddAsync(PollRequest request);
		Task<Result> UpdateAsync(int id, PollRequest request);
		Task<Result> DeleteAsync(int id);
		Task<Result> TogglePublishStatusAsync(int id);
	}
	public class PollService(ApplicationDbContext db) : IPollService
	{
		private readonly ApplicationDbContext _db = db;

		public async Task<Result<PollResponse>> GetAsync(int id)
		{
			Poll? poll = await _db.Polls.FindAsync(id);

			if (poll is null)
				return Result.Failure<PollResponse>(PollErrors.PollNotFound);

			return Result.Success(poll.Adapt<PollResponse>());
		}

		public async Task<Result<List<PollResponse>>> GetAllAsync()
		{
			var polls = await _db.Polls.AsNoTracking().ToListAsync();

			return Result.Success(polls.Adapt<List<PollResponse>>());
		}

		public async Task<Result<PollResponse>> AddAsync(PollRequest request)
		{
			var poll = await _db.Polls.AddAsync(request.Adapt<Poll>());
			await _db.SaveChangesAsync();

			return Result.Success(poll.Entity.Adapt<PollResponse>());
		}

		public async Task<Result> UpdateAsync(int id, PollRequest request)
		{
			var poll = await _db.Polls.FindAsync(id);

			if (poll is null)
				return Result.Failure(PollErrors.PollNotFound);

			poll.Title = request.Title;
			poll.Summary = request.Summary;
			poll.StartsAt = request.StartsAt;
			poll.EndsAt = request.EndsAt;

			await _db.SaveChangesAsync();

			return Result.Success();
		}

		public async Task<Result> DeleteAsync(int id)
		{
			var poll = await _db.Polls.FindAsync(id);

			if (poll is null)
				return Result.Failure(PollErrors.PollNotFound);

			_db.Polls.Remove(poll);
			await _db.SaveChangesAsync();

			return Result.Success();
		}

		public async Task<Result> TogglePublishStatusAsync(int id)
		{
			var poll = await _db.Polls.FindAsync(id);

			if (poll is null)
				return Result.Failure(PollErrors.PollNotFound);

			poll.IsPublished = !poll.IsPublished;
			await _db.SaveChangesAsync();

			return Result.Success();
		}
	}
}