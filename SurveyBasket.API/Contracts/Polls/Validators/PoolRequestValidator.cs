using FluentValidation;
using SurveyBasket.API.Contracts.Polls.Requests;

namespace SurveyBasket.API.Contracts.Polls.Validators
{
	public class LoginRequestValidator : AbstractValidator<PollRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(e => e.Title)
				.NotEmpty()
				.Length(3, 100);

			RuleFor(e => e.Summary)
				.NotEmpty()
				.Length(3, 1000);

			RuleFor(x => x.StartsAt)
			.NotEmpty()
			.GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

			RuleFor(x => x.EndsAt)
			.NotNull();
			//.GreaterThan(x => x.StartsAt);
			RuleFor(x => x)
				.Must(hasValidDates)
				.WithName("EndsAt")
				.WithMessage("{PropertyName} munst be greater than or equals StartsAt");


		}

		private bool hasValidDates(PollRequest pollRequest)
		{
			return pollRequest.EndsAt >= pollRequest.StartsAt;
		}
	}
}
