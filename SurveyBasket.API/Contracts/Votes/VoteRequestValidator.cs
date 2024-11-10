using FluentValidation;

namespace SurveyBasket.API.Contracts.Votes
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
