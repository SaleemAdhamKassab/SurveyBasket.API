using FluentValidation;
using SurveyBasket.API.Contracts.Votes.Requests;

namespace SurveyBasket.API.Contracts.Votes.Validators
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
