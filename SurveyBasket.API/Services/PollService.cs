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
		Task<List<PollResponse>> GetAllAsync();
		Task<List<PollResponse>> GetCurrentAsync();
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
			var poll = await _db.Polls.FindAsync(id);

			if (poll is null)
				return Result.Failure<PollResponse>(PollErrors.PollNotFound);

			var result = new PollResponse
			{
				Id = poll.Id,
				Title = poll.Title,
				Summary = poll.Summary,
				IsPublished = poll.IsPublished,
				StartsAt = poll.StartsAt,
				EndsAt = poll.EndsAt
			};

			return Result.Success(result);
		}
		public async Task<List<PollResponse>> GetCurrentAsync()
		{
			var currentPolls = await _db.Polls
				.Where(e => e.IsPublished)
				//e.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) &&
				//e.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
				.Select(e => new PollResponse
				{
					Id = e.Id,
					Title = e.Title,
					Summary = e.Summary,
					IsPublished = e.IsPublished,
					StartsAt = e.StartsAt,
					EndsAt = e.EndsAt
				})
				.AsNoTracking()
				.ToListAsync();

			return currentPolls;
		}
		public async Task<List<PollResponse>> GetAllAsync()
		{
			var result = await _db.Polls
				.Select(e => new PollResponse
				{
					Id = e.Id,
					Title = e.Title,
					Summary = e.Summary,
					IsPublished = e.IsPublished,
					StartsAt = e.StartsAt,
					EndsAt = e.EndsAt
				})
				.AsNoTracking()
				.ToListAsync();

			return result;
		}

		public async Task<Result<PollResponse>> AddAsync(PollRequest request)
		{
			bool isExsistingTitle = await _db.Polls
				.AnyAsync(e => e.Title.Trim().ToLower() == request.Title.Trim().ToLower());

			if (isExsistingTitle)
				return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

			var poll = new Poll
			{
				Title = request.Title,
				Summary = request.Summary,
				StartsAt = request.StartsAt,
				EndsAt = request.EndsAt,
				IsPublished = true
			};

			await _db.Polls.AddAsync(poll);
			await _db.SaveChangesAsync();

			var result = new PollResponse
			{
				Id = poll.Id,
				Title = poll.Title,
				Summary = poll.Summary,
				IsPublished = poll.IsPublished,
				StartsAt = poll.StartsAt,
				EndsAt = poll.EndsAt
			};

			return Result.Success(result);
		}

		public async Task<Result> UpdateAsync(int id, PollRequest request)
		{
			bool isExsistingTitle = await _db.Polls.AnyAsync(e => e.Title.Trim().ToLower() == request.Title.Trim().ToLower() && e.Id != id);

			if (isExsistingTitle)
				return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

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