using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Answers;
using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Data;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using System.Threading;

namespace SurveyBasket.API.Services
{
	public interface IQuestionService
	{
		Task<Result<QuestionResponse>> GetAsync(int pollId, int id);
		Task<Result<List<QuestionResponse>>> GetAllAsync(int pollId);
		Task<Result<List<QuestionResponse>>> GetAvailableAsync(int pollId, string userId);
		Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
		Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request);
		Task<Result> ToggleStatusAsync(int pollId, int id);
	}

	public class QuestionService(ApplicationDbContext db) : IQuestionService
	{
		private readonly ApplicationDbContext _db = db;

		public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id)
		{

			var isPollExists = await _db.Polls.AnyAsync(e => e.Id == pollId);
			if (!isPollExists)
				return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);


			var question = await _db.Questions
				.Where(e => e.PollId == pollId && e.Id == id)
				.Select(e => new QuestionResponse
				{
					Id = e.Id,
					Content = e.Content,
					Answers = e.Answers.Select(a => new AnswerResponse
					{
						Id = a.Id,
						Content = a.Content
					})
				})
				.AsNoTracking()
				.SingleOrDefaultAsync();

			if (question is null)
				return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

			return Result.Success(question);
		}

		public async Task<Result<List<QuestionResponse>>> GetAllAsync(int pollId)
		{
			var pollIsExists = await _db.Polls.AnyAsync(e => e.Id == pollId);

			if (!pollIsExists)
				return Result.Failure<List<QuestionResponse>>(PollErrors.PollNotFound);

			var questions = await _db.Questions
				.Where(e => e.PollId == pollId)
				.Select(e => new QuestionResponse
				{
					Id = e.Id,
					Content = e.Content,
					Answers = e.Answers.Select(a => new AnswerResponse
					{
						Id = a.Id,
						Content = a.Content
					})
				})
				.AsNoTracking()
				.ToListAsync();

			return Result.Success(questions);
		}
		public async Task<Result<List<QuestionResponse>>> GetAvailableAsync(int pollId, string userId)
		{
			var hasVote = await _db.Votes.AnyAsync(e => e.PollId == pollId && e.UserId == userId);

			if (hasVote)
				return Result.Failure<List<QuestionResponse>>(VoteErrors.DuplicatedVote);

			var pollisExsits = await _db.Polls.AnyAsync(e => e.Id == pollId && e.IsPublished);

			if (!pollisExsits)
				return Result.Failure<List<QuestionResponse>>(PollErrors.PollNotFound);


			var result = await _db.Questions
				.Where(e => e.PollId == pollId && e.IsActive)
				.Select(e => new QuestionResponse
				{
					Id = e.Id,
					Content = e.Content,
					Answers = e.Answers
								.Where(a => a.IsActive)
								.Select(a => new AnswerResponse
								{
									Id = a.Id,
									Content = a.Content
								})
				})
				.AsNoTracking()
				.ToListAsync();

			return Result.Success(result);
		}
		public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request)
		{
			var pollIsExists = await _db.Polls.AnyAsync(e => e.Id == pollId);

			if (!pollIsExists)
				return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

			bool questionIsExists = _db.Questions
				.Any(e => e.Content.Trim().ToLower() == request.Content.Trim().ToLower() && e.PollId == pollId);

			if (questionIsExists)
				return Result.Failure<QuestionResponse>(PollErrors.DuplicatedPollTitle);

			var question = new Question
			{
				PollId = pollId,
				Content = request.Content,
				IsActive = true,
			};

			request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

			await _db.Questions.AddAsync(question);
			await _db.SaveChangesAsync();


			var result = new QuestionResponse
			{
				Id = question.Id,
				Content = question.Content,
				Answers = question.Answers.Select(a => new AnswerResponse
				{
					Id = a.Id,
					Content = a.Content
				})
			};

			return Result.Success(result);
		}
		public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request)
		{
			var pollIsExists = await _db.Polls.AnyAsync(e => e.Id == pollId);

			if (!pollIsExists)
				return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

			var questionIsExists = await _db.Questions
			.AnyAsync(x => x.PollId == pollId
				&& x.Id != id
				&& x.Content == request.Content
			);

			if (questionIsExists)
				return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

			var question = await _db.Questions
				.Include(x => x.Answers)
				.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id);

			if (question is null)
				return Result.Failure(QuestionErrors.QuestionNotFound);

			question.Content = request.Content;

			//current answers
			var currentAnswers = question.Answers.Select(x => x.Content).ToList();

			//add new answer
			var newAnswers = request.Answers.Except(currentAnswers).ToList();

			newAnswers.ForEach(answer =>
			{
				question.Answers.Add(new Answer { Content = answer });
			});

			question.Answers.ToList().ForEach(answer =>
			{
				answer.IsActive = request.Answers.Contains(answer.Content);
			});

			await _db.SaveChangesAsync();

			return Result.Success();
		}
		public async Task<Result> ToggleStatusAsync(int pollId, int id)
		{
			var question = await _db.Questions.SingleOrDefaultAsync(e => e.PollId == pollId && e.Id == id);

			if (question is null)
				return Result.Failure(QuestionErrors.QuestionNotFound);

			question.IsActive = !question.IsActive;
			await _db.SaveChangesAsync();

			return Result.Success();
		}
	}
}
