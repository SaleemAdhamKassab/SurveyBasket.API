using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Contracts.Responses;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services
{
	public interface IPollService
	{
		Task<List<Poll>> GetAllAsync();
		Task<Poll>? GetAsync(int id);
		Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default);
		Task<bool> UpdateAsync(int id, Poll poll);
		Task<bool> DeleteAsync(int id);
	}
	public class PollService(ApplicationDbContext db) : IPollService
	{
		private readonly ApplicationDbContext _db = db;

		public async Task<List<Poll>> GetAllAsync()
		{
			List<Poll> polls = await _db.Polls.AsNoTracking().ToListAsync();
			return polls;
		}

		public async Task<Poll?> GetAsync(int id)
		{
			Poll poll = await _db.Polls.FindAsync(id);
			return poll;
		}

		public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
		{
			await _db.Polls.AddAsync(poll, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
			return poll;
		}

		public async Task<bool> UpdateAsync(int id, Poll poll)
		{
			Poll pollToUpdate = await GetAsync(id);

			if (pollToUpdate is null)
				return false;

			pollToUpdate.Title = poll.Title;
			pollToUpdate.Summary = poll.Summary;
			pollToUpdate.IsPublished = poll.IsPublished;
			pollToUpdate.StartsAt = poll.StartsAt;
			pollToUpdate.EndsAt = poll.EndsAt;

			await _db.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Poll pollToDelete = await GetAsync(id);

			if (pollToDelete is null)
				return false;

			_db.Polls.Remove(pollToDelete);
			await _db.SaveChangesAsync();
			return true;
		}
	}
}