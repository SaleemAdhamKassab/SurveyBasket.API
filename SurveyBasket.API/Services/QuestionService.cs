using Azure.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Answers;
using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Data;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services
{
	public interface IQuestionService
	{
		Task<Result<QuestionResponse>> GetAsync(int pollId, int id);
		Task<Result<List<QuestionResponse>>> GetAllAsync(int pollId);
		Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request);
		Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request);
		Task<Result> ToggleStatusAsync(int pollId, int id);
	}

	public class QuestionService(ApplicationDbContext db) : IQuestionService
	{
		private readonly ApplicationDbContext _db = db;

		public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id)
		{
			var question = await _db.Questions
				.Where(e => e.PollId == pollId && e.Id == id)
				.Include(e => e.Answers)
				.ProjectToType<QuestionResponse>()
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
				.Include(e => e.Answers)
				//.Select(e => new QuestionResponse
				//(
				//	e.Id,
				//	e.Content,
				//	e.Answers.Select(a => new AnswerResponse(
				//		a.Id, a.Content
				//	))
				//))
				.ProjectToType<QuestionResponse>() // using mappster
				.AsNoTracking()
				.ToListAsync();

			return Result.Success(questions);
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

			var question = request.Adapt<Question>();
			question.PollId = pollId;

			// solved by mappster
			//request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));


			await _db.Questions.AddAsync(question);
			await _db.SaveChangesAsync();

			return Result.Success(question.Adapt<QuestionResponse>());
		}
		public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request)
		{
			var questenIsExists = await _db.Questions.AnyAsync(e => e.PollId == pollId && e.Id != id && e.Content == request.Content);

			if (questenIsExists)
				return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

			var question = await _db.Questions
				.Include(e => e.Answers)
				.SingleOrDefaultAsync(e => e.PollId == pollId && e.Id == id);

			if (question is null)
				return Result.Failure(QuestionErrors.QuestionNotFound);

			question.Content = request.Content;

			// Update Question Answers has 2 cases:
			var currentAnswers = question.Answers.Select(e => e.Content).ToList();

			//1) add new answers from request
			// using Except to get answers from request and Not created in databse
			var newAnswers = request.Answers.Except(currentAnswers).ToList();
			newAnswers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

			//2) if db answer not include in new request answers -> then disable it 
			question.Answers.ToList().ForEach(answer =>
			{
				answer.IsActive = request.Content.Contains(answer.Content);
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
