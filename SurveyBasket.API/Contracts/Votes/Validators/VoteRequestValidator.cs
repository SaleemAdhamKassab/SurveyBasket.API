using FluentValidation;
using SurveyBasket.API.Contracts.Votes.Requests;

namespace SurveyBasket.API.Contracts.Votes.Validators
{
	public class VoteRequestValidator : AbstractValidator<VoteRequest>
	{
		public VoteRequestValidator()
		{
			RuleFor(e => e.VoteAnswers).NotEmpty();

			RuleForEach(e => e.VoteAnswers)
				.SetInheritanceValidator(v => v.Add(new VoteAnswerRequestValidator()));
		}
	}
}
