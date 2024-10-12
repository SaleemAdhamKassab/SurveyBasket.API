using FluentValidation;
using SurveyBasket.API.Contracts.Requests;

namespace SurveyBasket.API.Contracts.Validations
{
	public class PoolRequestValidator : AbstractValidator<PollRequest>
	{
		public PoolRequestValidator()
		{
			RuleFor(e => e.Title)
				.NotEmpty()
				.Length(3, 100);

			RuleFor(e => e.Summary)
				.NotEmpty()
				.Length(3, 1000);
		}
	}
}
