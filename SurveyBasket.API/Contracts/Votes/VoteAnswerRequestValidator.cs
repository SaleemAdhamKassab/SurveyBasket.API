using FluentValidation;

namespace SurveyBasket.API.Contracts.Votes
{
	public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
	{
		public VoteAnswerRequestValidator()
		{
			RuleFor(e => e.QuestionId).GreaterThan(0);
			RuleFor(e => e.AnswerId).GreaterThan(0);
		}
	}
}
